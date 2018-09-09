using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace react_dot_app.Db 
{
    
    static class DbConfigurationManager
    {
        public static IConfiguration DbSetting { get; }
        static DbConfigurationManager() => DbSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("dbsettings.json")
                    .Build();
    }
}