using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using react_dot_app.Models;

namespace react_dot_app.Db 
{
    /// .summary
    /// Static class for performing Database queries using a Neo4j GraphDB
    /// Queries are written in "Cypher" language and use Neo4jClient package
    /// as the connector.
    public static class Neo4jDb
    {
        private static readonly string user = DbConfigurationManager.DbSetting["Neo4j:User"];
        private static readonly string password = DbConfigurationManager.DbSetting["Neo4j:Password"];
        public static GraphClient Client = new GraphClient(
            new Uri(DbConfigurationManager.DbSetting["Neo4j:URI"]), user, password);

        public static IEnumerable<Band> GetAllBands() {
            
            Console.WriteLine("Loading Neo4j data");
            Client.Connect();

            return Client.Cypher
                .Match("(band:Band)")
                .Return(band => band.As<Band>())
                .Results;    
        }

        public static IEnumerable<Band> GetBandsByName(string name) {

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

        public static IEnumerable<Band> GetBandsByRating(int rating) {

            Console.WriteLine("Getting by Rating");
            Client.Connect();

            return Client.Cypher
                .Match("(band:Band)")
                .Where("band.rating >= {rating}")
                .WithParam("rating", rating)
                .Return(band => band.As<Band>())
                .Results;
        }

        public static IEnumerable<Band> GetBandsByGenre(string genre) {

            Console.WriteLine("Getting by Genre");
            Client.Connect();

            return Client.Cypher
                .Match("(band:Band)-[:HAS_GENRE]-(genre:Genre)")
                .Where("(genre.name) = {genre}")
                .WithParam("genre", genre)
                .Return(band => band.As<Band>())
                .Results;
        }

        public static Task<IEnumerable<Band>> AddBandAsync(Band newBand)
        {
            Console.WriteLine("Adding Band:" + newBand.Name);
            Client.Connect();

            return Client.Cypher
                .Merge("(band:Band { Name: {name}})")
                .OnCreate()
                .Set("band = {newBand}")
                .WithParams(new {
                    name = newBand.Name,
                    newBand
                })
                .Return(band => band.As<Band>())
                .ResultsAsync;
        }
    }

}