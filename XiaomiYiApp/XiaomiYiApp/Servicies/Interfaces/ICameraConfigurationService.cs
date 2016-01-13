using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Entities;

namespace XiaomiYiApp.Servicies.Interfaces
{
    public interface ICameraConfigurationService
    {
        OperationResult<Configuration> GetConfiguration();

        OperationResult<DetailedConfiguration> GetDetailedConfiguration();

        OperationResult SetConfigurationParameter(String name, String value);

         Task<OperationResult<Configuration>> GetConfigurationAsync();
       
         Task<OperationResult<DetailedConfiguration>> GetDetailedConfigurationAsync();
       
         Task<OperationResult> SetConfigurationParameterAsync(String name, String value);

         Task<OperationResult<float>> GetSdCardFreeSpaceAsync();

         Task<OperationResult<float>> GetSdCardSizeAsync();

         Task<OperationResult<BatteryInfo>> GetBatteryInfoAsync();

    }
}
