using ServiceTemplate.Business.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTemplate.Business.Configuration
{
    public static class Config
    {
        #region Attributes
        private const string _className = "Config";
        private static IConfiguration? _config;
        #endregion

        #region Methods
        public static void LoadConfig()
        {
            try
            {
                // 1. Load appsettings.json
                _config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                // 2. Get Log configurations
                string logDirectory = _config["AppLogging:LogDirectory"] ?? "logs".Replace(@"/", "\\");

                // 3. Configure Serilog and initialize logger
                Logger.InitLogger(logDirectory);
                Logger.Info(_className, "LoadConfig", "Logger initialized, loading settings...");

            } 
            catch (Exception ex)
            {
                throw new Exception("An error occurred while loading application settings!", ex);
            }
        }

        /// <summary>
        /// Get configuration parameter by key, if key is an object than parameter = "key:attribute" format
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string Get(string parameter)
        {
            try
            {
                return _config?[parameter]!;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "Get", $"{ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
