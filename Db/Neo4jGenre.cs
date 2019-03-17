using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Neo4jClient;
using Neo4jClient.Transactions;
using react_dot_app.Models;

namespace react_dot_app.Db 
{
    public static class Neo4jGenre
    {
        private readonly static IGraphClient Client = Neo4jDbClient.Client;
        public static IEnumerable<Genre> GetAllGenres()
        {
            Client.Connect();
            return Client.Cypher
                .Match("(genre: Genre)")
                .Return(genre => genre.As<Genre>())
                .Results;
        }

        public static IEnumerable<Genre> GetGenresByBand(string bandName)
        {
            Client.Connect();
            return Client.Cypher
                .Match("(genre:Genre)-[:HAS_GENRE]-(band:Band)")
                .Where("(band.name) = {name}")
                .WithParam("name", bandName)
                .Return(genre => genre.As<Genre>())
                .Results;
        }
    }
}