using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceTemplate.Business.Logging;
using ServiceTemplate.Business.Configuration;
using ServiceTemplate.Business.Interfaces;

namespace ServiceTemplate.Business
{
    public class ServiceProcessingEngine : IServiceProcessingEngine
    {
        #region Attributes
        private const string _className = "ServiceProcessingEngine";
        #endregion

        #region Dependencies
        #endregion

        public ServiceProcessingEngine()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.Error(_className, "ServiceProcessingEngine constructor", $"Error: {ex.Message}");
                throw;
            }
        }

        #region Methods
        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(Config.Interval), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Logger.Debug(_className, "ProcessAsync", "Application termination by signaling and cancellation token");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "ProcessAsync", $"Error: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
