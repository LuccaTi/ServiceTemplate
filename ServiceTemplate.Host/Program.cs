using ServiceTemplate.Business;
using ServiceTemplate.Business.Configuration;
using ServiceTemplate.Business.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using Topshelf;

namespace ServiceTemplate.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Config.LoadConfig();

                // Start TopShelf
                var exitCode = HostFactory.Run(hostConfig =>
                {
                    hostConfig.Service<ServiceWork>(config =>
                    {
                        config.ConstructUsing(work => new ServiceWork());
                        config.WhenStarted((work, _) =>
                        {
                            Logger.Info("Program.cs", "WhenStarted", "Starting application...");
                            work.Start();
                            return true;
                        });

                        config.WhenStopped((work, _) =>
                        {
                            work.Stop();
                            Logger.Info("Program.cs", "WhenStopped", $"Application terminated!");
                            return true;
                        });
                    });

                    hostConfig.RunAsLocalSystem();
                    hostConfig.SetServiceName("Template");
                    hostConfig.SetDisplayName("Template");
                    hostConfig.SetDescription("Service template used to create Windows services.");

                });

                // For generic cases: (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
                int exitCodeValue = (int)exitCode;
                Environment.ExitCode = exitCodeValue;
                Console.WriteLine($"ExitCode: {Environment.ExitCode}");
            }
            catch (Exception ex)
            {
                // Terminates the service when errors are not caught by Topshelf at startup
                HandleStartupError(ex);
                Console.WriteLine($"{DateTime.Now} - Error: {ex}");
                Environment.Exit(1);
            }

        }

        private static void HandleStartupError(Exception exception)
        {
            // Creates a file due to the chance that the Logger may not have been initialized

            string fatalErrorDirectory = Path.Combine(AppContext.BaseDirectory, "StartupErrors");
            if (!Directory.Exists(fatalErrorDirectory))
                Directory.CreateDirectory(fatalErrorDirectory);

            string timeStamp = DateTime.Now.Date.ToString("yyyyMMdd");
            string file = Path.Combine(fatalErrorDirectory, $"{timeStamp}_ERROR_.txt");
            string errorMsg = $"{DateTime.Now} - Error during application startup: {exception.ToString()}{Environment.NewLine}";
            File.AppendAllText(file, errorMsg);
        }
    }
}
