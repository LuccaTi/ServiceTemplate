using Service.Business;
using Service.Business.Configuration;
using Service.Business.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using Topshelf;

namespace Service.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Config.LoadConfig();

                // Inicia o TopShelf
                var exitCode = HostFactory.Run(hostConfig =>
                {
                    hostConfig.Service<ServiceWork>(serviceConfig =>
                    {
                        serviceConfig.ConstructUsing(serviceWork => new ServiceWork());
                        serviceConfig.WhenStarted((serviceWork, _) =>
                        {
                            Logger.Info("Program.cs", "WhenStarted", "Iniciando serviço...");
                            serviceWork.StartService();
                            return true;
                        });

                        serviceConfig.WhenStopped((serviceWork, _) =>
                        {
                            serviceWork.StopService();
                            Logger.Info("Program.cs", "WhenStopped", $"Serviço encerrado!");
                            return true;
                        });
                    });

                    hostConfig.RunAsLocalSystem();
                    hostConfig.SetServiceName("ServiceTemplate");
                    hostConfig.SetDisplayName("ServiceTemplate");
                    hostConfig.SetDescription("Modelo de serviço usado para criar serviços do windows.");

                });

                // Para casos genéricos: (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
                int exitCodeValue = (int)exitCode;
                Environment.ExitCode = exitCodeValue;
                Console.WriteLine($"ExitCode: {Environment.ExitCode}");
            }
            catch (Exception ex)
            {
                // Encerra o serviço diante de erros não pegos pelo Topshelf no startup
                HandleStartupError(ex);
                Console.WriteLine($"{DateTime.Now} - Erro: {ex}");
                Environment.Exit(1);
            }

        }

        private static void HandleStartupError(Exception exception)
        {
            // Cria um arquivo devido a chance do Logger não ter sido iniciado

            string fatalErrorDirectory = Path.Combine(AppContext.BaseDirectory, "StartupErrors");
            if (!Directory.Exists(fatalErrorDirectory))
                Directory.CreateDirectory(fatalErrorDirectory);

            string timeStamp = DateTime.Now.Date.ToString("yyyyMMdd");
            string file = Path.Combine(fatalErrorDirectory, $"{timeStamp}_ERROR_.txt");
            string errorMsg = $"{DateTime.Now} - Erro durante o startup do serviço: {exception.ToString()}{Environment.NewLine}";
            File.AppendAllText(file, errorMsg);
        }
    }
}
