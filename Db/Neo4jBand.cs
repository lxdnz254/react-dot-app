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
    public static class Neo4jBand 
    {
        private readonly static IGraphClient Client = Neo4jDbClient.Client;
        private static Task<IEnumerable<Band>> queryBuilder;

        public static IEnumerable<Band> GetAllBands() 
        {  
            Console.WriteLine("Loading Neo4j data");
            Client.Connect();
            return Client.Cypher
                .Match("(band:Band)")
                .Return(band => band.As<Band>())
                .Results;    
        }

        public static IEnumerable<Band> GetBandsByName(string name) 
        {
            Console.WriteLine("Getting By Name");
            Client.Connect();
            return Client.Cypher
                .Match("(band:Band)")
                .Where("band.name STARTS WITH {name} OR band.name STARTS WITH {upperName}")
                .WithParams(new {name = name.ToLower(), 
                                upperName = name[0].ToString().ToUpper() + name.Substring(1, name.Length - 1)})
                .Return(band => band.As<Band>())
                .Results;
        }

        public static IEnumerable<Band> GetBandsByRating(int rating) 
        {
            Console.WriteLine("Getting by Rating");
            Client.Connect();
            return Client.Cypher
                .Match("(band:Band)")
                .Where("band.rating >= {rating}")
                .WithParam("rating", rating)
                .Return(band => band.As<Band>())
                .Results;
        }

        public static IEnumerable<Band> GetBandsByGenre(string genre) 
        {
            Console.WriteLine("Getting by Genre");
            Client.Connect();
            return Client.Cypher
                .Match("(band:Band)-[:HAS_GENRE]-(genre:Genre)")
                .Where("(genre.name) = {genre}")
                .WithParam("genre", genre)
                .Return(band => band.As<Band>())
                .Results;
        }

        public static IEnumerable<Band> GetBandsByCost(int cost) 
        {
            Console.WriteLine("Getting Bands by Cost");
            Client.Connect();
            return Client.Cypher
                .Match("(band:Band)")
                .Where("(band.cost) >= {cost}")
                .WithParam("cost", cost)
                .Return(band => band.As<Band>())
                .Results;
        }

        public static async Task<IEnumerable<Band>> AddBandAsync(
            Band newBand, 
            List<Genre> genres, 
            List<Member> members)
        {
            Console.WriteLine("Adding Band: " + newBand.Name);
            var gNames = new List<String>();
            var mNames = new List<String>();
            var mInstruments = new List<String>();
            foreach(var genre in genres)
            {
                gNames.Add(genre.Name);
            }
            
            foreach(var member in members)
            {
                mNames.Add(member.Name);
                mInstruments.Add(member.Instrument);
            }
            Client.Connect();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                queryBuilder = Client.Cypher
                    .Merge("(band:Band { name: {name}})")
                    .OnCreate()
                    .Set("band = {newBand}")
                    .ForEach("(genreName IN {gNames} | MERGE (g:Genre {name: genreName}) MERGE (band)-[:HAS_GENRE]-(g))")
                    .ForEach("(mName IN {mNames} | MERGE (m:Member {name: mName}) MERGE (m)-[:IN_BAND]-(band))")
                    .WithParams(new {
                        name = newBand.Name,
                        newBand,
                        gNames,
                        mNames
                    })
                    .Return(band => band.As<Band>())
                    .ResultsAsync;

                for(int i = 0; i < mNames.Count; i++)
                {
                    Console.WriteLine("Instrument: " + mInstruments[i]);
                    await Client.Cypher
                        .Match("(mem:Member {name: {name}})-[:IN_BAND]-(b:Band)")
                        .Where("(b.name) = {bandName}")
                        .Set("mem.instrument = {instrument}")
                        .WithParams(new {
                            bandName = newBand.Name,
                            name = mNames[i],
                            instrument = mInstruments[i]
                        })
                        .ExecuteWithoutResultsAsync();
                }
                scope.Complete();
            }
            return await queryBuilder;
        }
    }
}