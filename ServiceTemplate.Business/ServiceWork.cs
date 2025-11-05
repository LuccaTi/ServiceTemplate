using Service.Business.Configuration;
using Service.Business.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Reflection.Metadata.Ecma335;
using System.Timers;


namespace Service.Business
{
    public class ServiceWork
    {
        #region Atributes
        private const string _className = "ServiceWork";
        private System.Timers.Timer _timer;
        #endregion

        public ServiceWork()
        {
            try
            {
                var interval = int.Parse(Config.Get("AppConfig:Interval"));
                Logger.Info(_className, "Constructor", "Configurações carregadas!");

                // Intervalo usado antes de iniciar o trabalho
                _timer = new System.Timers.Timer(interval);
                _timer.Elapsed += CreateWorkThreads;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "Constructor", $"Erro: {ex.ToString()}{Environment.NewLine}Aplicação será encerrada pelo TopShelf!");
                throw;
            }
        }

        #region Methods
        private void CreateWorkThreads(object? sender, ElapsedEventArgs e)
        {
            try
            {
                // Implementação de threads que vão executar as funcionalidades da aplicação
                // Task.Run ou new Thread();
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "CreateWorkThreads", $"Erro: {ex.Message}");
                throw;
            }
        }
        public void Start()
        {
            try
            {
                Logger.Info(_className, "Start", "Aplicação iniciada com sucesso!");
                _timer.Start();
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "Start", $"Erro: {ex.Message}");
                throw;
            }

        }
        public void Stop()
        {
            try
            {
                Logger.Info(_className, "Stop", "Requisição para finalizar recebida, parando aplicação...");
                _timer.Stop();
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "Stop", $"Erro: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
