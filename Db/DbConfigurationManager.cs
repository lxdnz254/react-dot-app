using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace react_dot_app.Db 
{
    ///.summary
    /// any Database settings should be stored in "dbsettings.json" at the top
    /// level of the application. "dbsettings.json" should be ignored in .gitignore
    /// this file may store senstive information like passwords and usernames .see Neo4jDb.cs
    static class DbConfigurationManager
    {
        public static IConfiguration DbSetting { get; }
        static DbConfigurationManager() => DbSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("dbsettings.json")
                    .Build();
    }
}