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
using XiaomiYiApp.Model.Events;

namespace XiaomiYiApp.Repositories
{
    class CaneraStateRepository : ICaneraStateRepository
    {
        private CameraState _currentState;

        private IEventAggregator _eventAggregator;
        private ICameraConfigurationService _configurationService;

        public Boolean IsLoaded
        {
            get
            {
                return _currentState != null;
            }
        }

        public CaneraStateRepository(IEventAggregator eventAggregator, ICameraConfigurationService configurationService)
        {
            _eventAggregator = eventAggregator;
            _configurationService = configurationService;
            _eventAggregator.GetEvent<CameraSystemModeChangedEvent>().Subscribe((mode) =>
                                                                                        {
                                                                                            if (IsLoaded)
                                                                                            {
                                                                                                _currentState.SystemMode = mode;
                                                                                            }
                                                                                        });
            _eventAggregator.GetEvent<BatteryInfoChangedEvent>().Subscribe((batteryInfo) =>
            {
                if (IsLoaded)
                {
                    _currentState.Battery = batteryInfo;
                }
            });
        }

        public async Task<OperationResult> LoadCameraStateAsync()
        {
            //CameraState currentState = new CameraState();

           var batteryTask = await _configurationService.GetBatteryInfoAsync();

            var confTask = await _configurationService.GetConfigurationAsync();
            var sdCardTask = await _configurationService.GetSdCardInfoAsync();




            if (!(confTask).Success)
            {
                return OperationResult.FromResult(confTask);
            }
            if (!(sdCardTask).Success)
            {
                return OperationResult.FromResult(sdCardTask);
            }
            if (!(batteryTask).Success)
            {
                return OperationResult.FromResult(sdCardTask);
            }

            _currentState = new CameraState();
            _currentState.RecordMode = confTask.Result.GetValue(ConfigurationParameterName.REC_MODE).GetEnumFromDescription<CameraRecordMode>();
            _currentState.CaptureMode = confTask.Result.GetValue(ConfigurationParameterName.CAPTURE_MODE).GetEnumFromDescription<CameraCaptureMode>();
            _currentState.SystemMode = confTask.Result.GetValue(ConfigurationParameterName.SYSTEM_MODE).GetEnumFromDescription<CameraSystemMode>();
            _currentState.Storage = sdCardTask.Result;
            _currentState.Battery = batteryTask.Result;
                //new BatteryInfo { BatteryLevel = 100, BatteryStatus = BatteryStatus.InUse }; 

            return new OperationResult { Success = true };
        }

       

        public CameraState GetCurrentCameraState()
        {
            return _currentState;
        }

        public async Task<OperationResult> SetSystemModeAsync(CameraSystemMode systemMode)
        {
            _currentState.SystemMode = systemMode;
            return await _configurationService.SetConfigurationParameterAsync(ConfigurationParameterName.SYSTEM_MODE, systemMode.GetDescription());
        }

        public async Task<OperationResult> SetRecordModeAsync(CameraRecordMode recordMode)
        {
            _currentState.RecordMode = recordMode;
            return await _configurationService.SetConfigurationParameterAsync(ConfigurationParameterName.REC_MODE, recordMode.GetDescription());
        }

        public async Task<OperationResult> SetCaptureModeAsync(CameraCaptureMode captureMode)
        {
            _currentState.CaptureMode = captureMode;
            return await _configurationService.SetConfigurationParameterAsync(ConfigurationParameterName.CAPTURE_MODE, captureMode.GetDescription());
        }

        public async Task<OperationResult> SetAppAcquisitionMode(CameraAppAcquisitionMode acquisitionMode)
        {
            if (_currentState.AppAcquisitionMode == acquisitionMode)
            {
                return new OperationResult(true);
            }

            OperationResult opRes;// = new OperationResult { Success = true, };
            var newSystemMode = acquisitionMode.GetSystemMode();
            if (_currentState.SystemMode != newSystemMode)
            {
                opRes =  await SetSystemModeAsync(newSystemMode);
                if (!opRes.Success)
                {
                    return opRes;
                }
            }

            switch (newSystemMode)
            {
                case CameraSystemMode.Capture:
                    opRes = await SetCaptureModeAsync(acquisitionMode.GetCaptureMode());
                    break;
                case CameraSystemMode.Record:
                    opRes = await SetRecordModeAsync(acquisitionMode.GetRecordMode());
                    break;
                default:
                    throw new Exception("Unhandled CameraSystemMode");
                    //break;
            }

            return opRes;
        }
    }
}
