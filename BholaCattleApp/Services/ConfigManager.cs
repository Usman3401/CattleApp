using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.Services
{
    public static class ConfigManager
    {
        private static IConfiguration _configuration;

        public static string GetConnectionString(string name)
        {
            if (_configuration == null) 
            {
                _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            }

            return _configuration.GetConnectionString(name);
        }
    }
}
