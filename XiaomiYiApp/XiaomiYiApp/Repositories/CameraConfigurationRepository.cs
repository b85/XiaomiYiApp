using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Infrastrutture;
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Enums;
using XiaomiYiApp.Model.Messages;
using XiaomiYiApp.Repositories.Interfaces;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.Repositories
{
    public class CameraConfigurationRepository : ICameraConfigurationRepository
    {
        private ICameraConfigurationService _configurationService;
        private DetailedConfiguration _detailedConfigurationCache;

        public CameraConfigurationRepository(ICameraConfigurationService cameraConfigurationService)
        {
            _configurationService = cameraConfigurationService;
            _detailedConfigurationCache = null;
        }

        //public OperationResult<Configuration> GetConfigurationAsync()
        //{
           
        //   // return OperationResult<Configuration>.GetFail(opResult.ResultMessage);
        //}



        public async Task<OperationResult<DetailedConfiguration>> GetDetailedConfigurationAsync()
        {
            if (_detailedConfigurationCache != null)
            {
                var result = await _configurationService.GetConfigurationAsync();
                if (result.Success)
                {
                    foreach (var item in result.Result.Parameters)
                    {
                        _detailedConfigurationCache[item.Name].Value = item.Value;
                    }
                }

                return   OperationResult.GetSucces(_detailedConfigurationCache);
            }

            return await _configurationService.GetDetailedConfigurationAsync();
        }

        public async  Task<OperationResult> UpdateConfigurationParameterAsync(String name, String value)
        {
            var setParamResult = await _configurationService.SetConfigurationParameterAsync(name, value);
            if (setParamResult.Success)
            {
                UpdateConfigurationParameterCache(name, value);
            }

            return OperationResult.FromResult(setParamResult);
        }

        public async Task<OperationResult> LoadDetailedConfigurationAsync()
        {
            var getConfigResult = await _configurationService.GetDetailedConfigurationAsync();
            if (getConfigResult.Success)
            {
                _detailedConfigurationCache = getConfigResult.Result;
            }

            return OperationResult.FromResult(getConfigResult);
        }


        private void UpdateConfigurationParameterCache(String parameterName, String parameterValue)
        {
            if (_detailedConfigurationCache != null)
            {
                _detailedConfigurationCache[parameterName].Value = parameterValue;
            }
        }
    }
}
