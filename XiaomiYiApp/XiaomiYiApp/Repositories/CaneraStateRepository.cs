using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Enums;
using XiaomiYiApp.Repositories.Interfaces;
using XiaomiYiApp.Servicies.Interfaces;
using XiaomiYiApp.Infrastrutture;

namespace XiaomiYiApp.Repositories
{
    class CaneraStateRepository
    {
        private CameraState _currentState;

        private IEventAggregator _eventAggregator;
        private ICameraConfigurationService _configurationService;

        public async Task<OperationResult> LoadCameraStateAsync()
        {
            //CameraState currentState = new CameraState();
            var confTask = _configurationService.GetConfigurationAsync();
            var sdCardTask =  GetSdCardInfoAsync();
            var batteryTask = _configurationService.GetBatteryInfoAsync();
            if (!(await confTask).Success)
            {
               return OperationResult.FromResult(confTask.Result);
            }
            if (!(await sdCardTask).Success)
            {
                return OperationResult.FromResult(sdCardTask.Result);
            }
            if (!(await batteryTask).Success)
            {
                return OperationResult.FromResult(sdCardTask.Result);
            }


            _currentState = new CameraState();
            _currentState.VideoMode = confTask.Result.Result.GetValue("TODO").GetEnumFromDescription<CameraVideoMode>();
            _currentState.CaptureMode = confTask.Result.Result.GetValue("TODO").GetEnumFromDescription<CameraCaptureMode>();
            _currentState.SystemMode = confTask.Result.Result.GetValue("TODO").GetEnumFromDescription<CameraSystemMode>();
            _currentState.Storage = sdCardTask.Result.Result;
            _currentState.Battery = batteryTask.Result.Result;

            return OperationResult.FromResult(sdCardTask.Result);
        }

        public async Task<OperationResult<SdCardInfo>> GetSdCardInfoAsync()
        {
            //TODO
            return OperationResult<SdCardInfo>.GetSucces(null);
        }
    }
}
