using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Neo4jClient;
using Neo4jClient.Transactions;

namespace react_dot_app.Db 
{
    /// .summary
    /// Static class for performing Database queries using a Neo4j GraphDB
    /// Queries are written in "Cypher" language and use Neo4jClient package
    /// as the connector.
    public static class Neo4jDbClient
    {
        private static readonly string user = DbConfigurationManager.DbSetting["Neo4j:User"];
        private static readonly string password = DbConfigurationManager.DbSetting["Neo4j:Password"];
        public static GraphClient Client = new GraphClient(
            new Uri(DbConfigurationManager.DbSetting["Neo4j:URI"]), user, password);
    }
}