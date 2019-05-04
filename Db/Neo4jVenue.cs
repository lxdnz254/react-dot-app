using System;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Neo4jClient;
using Neo4jClient.Transactions;
using react_dot_app.Models;

namespace react_dot_app.Db 
{
    public static class Neo4jVenue 
    {
        private readonly static IGraphClient Client = Neo4jDbClient.Client;
        private static Task<IEnumerable<Venue>> queryBuilder;

        public static IEnumerable<Venue> GetAllVenues() 
        {  
            Console.WriteLine("Loading Neo4j data");
            Client.Connect();
            return Client.Cypher
                .Match("(venue:Venue)")
                .Return(venue => venue.As<Venue>())
                .Results;    
        }

        public static IEnumerable<Venue> GetVenuesByName(string name) 
        {
            Console.WriteLine("Getting By Name");
            Client.Connect();
            return Client.Cypher
                .Match("(venue:Venue)")
                .Where("venue.name STARTS WITH {name} OR venue.name STARTS WITH {upperName}")
                .WithParams(new {name = name.ToLower(), 
                                upperName = name[0].ToString().ToUpper() + name.Substring(1, name.Length - 1)})
                .Return(venue => venue.As<Venue>())
                .Results;
        }

        public static IEnumerable<Venue> GetVenuesByCapacity(int capacity) 
        {
            Console.WriteLine("Getting Venue by Capacity");
            Client.Connect();
            return Client.Cypher
                .Match("(venue:Venue)")
                .Where("venue.capacity >= {capacity}")
                .WithParam("capacity", capacity)
                .Return(venue => venue.As<Venue>())
                .Results;
        }

        public static IEnumerable<Venue> GetVenuesByGenre(string genre) 
        {
            Console.WriteLine("Getting Venues by Genre");
            Client.Connect();
            return Client.Cypher
                .Match("(venue:Venue)-[:LIKES_GENRES]-(genre:Genre)")
                .Where("(genre.name) = {genre}")
                .WithParam("genre", genre)
                .Return(venue => venue.As<Venue>())
                .Results;
        }

        public static IEnumerable<Venue> GetVenuesByLocation(string location) 
        {
            Console.WriteLine("Getting Venues by Location");
            Client.Connect();
            return Client.Cypher
                .Match("(venue:Venue)")
                .Where("(venue.location) STARTS WITH {location} OR venue.location STARTS WITH {upperLocation}")
                .WithParams(new {location = location.ToLower(), 
                                upperLocation = location[0].ToString().ToUpper()
                                + location.Substring(1, location.Length - 1)})
                .Return(venue => venue.As<Venue>())
                .Results;
        }

        public static async Task<IEnumerable<Venue>> AddVenueAsync(
            Venue newVenue, 
            List<Genre> genres)
        {
            Console.WriteLine("Adding Venue: " + newVenue.Name);
            var gNames = new List<String>();
            
            foreach(var genre in genres)
            {
                gNames.Add(genre.Name);
            }
            
            Client.Connect();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                queryBuilder = Client.Cypher
                    .Merge("(venue:Venue { name: {name}})")
                    .OnCreate()
                    .Set("venue = {newVenue}")
                    .ForEach("(genreName IN {gNames} | MERGE (g:Genre {name: genreName}) MERGE (venue)-[:LIKES_GENRES]-(g))")
                    .WithParams(new {
                        name = newVenue.Name,
                        newVenue,
                        gNames,
                    })
                    .Return(venue => venue.As<Venue>())
                    .ResultsAsync;

                scope.Complete();
            }
            return await queryBuilder;
        }
    }
}