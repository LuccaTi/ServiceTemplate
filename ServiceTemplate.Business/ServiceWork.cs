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
        #region Attributes
        private const string _className = "ServiceWork";
        private System.Timers.Timer _timer;
        #endregion

        public ServiceWork()
        {
            try
            {
                var interval = int.Parse(Config.Get("AppConfig:Interval"));
                Logger.Info(_className, "Constructor", "Settings loaded!");

                // Interval used before starting the work
                _timer = new System.Timers.Timer(interval);
                _timer.Elapsed += CreateWorkThreads;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "Constructor", $"Error: {ex.ToString()}{Environment.NewLine}Application will be terminated by TopShelf!");
                throw;
            }
        }

        #region Methods
        private void CreateWorkThreads(object? sender, ElapsedEventArgs e)
        {
            try
            {
                // Implementation of threads that will execute the application functionalities
                // Task.Run or new Thread();
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "CreateWorkThreads", $"Error: {ex.Message}");
                throw;
            }
        }
        public void Start()
        {
            try
            {
                Logger.Info(_className, "Start", "Application started successfully!");
                _timer.Start();
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "Start", $"Error: {ex.Message}");
                throw;
            }

        }
        public void Stop()
        {
            try
            {
                Logger.Info(_className, "Stop", "Request to stop received, stopping application...");
                _timer.Stop();
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "Stop", $"Error: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
