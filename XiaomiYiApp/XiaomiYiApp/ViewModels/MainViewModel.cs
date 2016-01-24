using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using PrismClone.StoreApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XiaomiYiApp.Infrastrutture;
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Enums;
using XiaomiYiApp.Model.Events;
using XiaomiYiApp.Repositories.Interfaces;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.ViewModels
{
    class MainViewModel : BindableBase , INavigationAware
    {
      
        private  IEventAggregator _eventAggregator;
        private INavigationService _navigationService;
        private ICaneraStateRepository _cameraStateRepository;
        private ICameraAcquisitionService _cameraAcquisitionService;

        private DelegateCommand _configurationCommand;
        private DelegateCommand _startAcquisitionCommand;
        private DelegateCommand _stopAcquisitionCommand;
        private CameraAppAcquisitionMode _selectedAcquisitionMode;
        private String _visualState;
        private CamereAppStatus _appStatus;

        public BatteryViewModel BatteryViewModel { get; private set; }
        public IEnumerable<CameraAppAcquisitionMode> AvailableAcquisitionMode { get; private set; }

        public String VisualState
        {
            get { return _visualState; }
            private set
            {
                _visualState = value;
                OnPropertyChanged("VisualState");
            }
        }

        public CameraAppAcquisitionMode SelectedAcquisitionMode
	    {
		    get { return _selectedAcquisitionMode;}
		    set 
            {
                if (_selectedAcquisitionMode != value)
                {
                    _cameraStateRepository.SetAppAcquisitionMode(value).ContinueWith((task) =>
                        { 
                            _selectedAcquisitionMode = value;
                            OnPropertyChanged("SelectedAcquisitionMode");
                           
                            //switch (_selectedAcquisitionMode)
                            //{
                            //    case CameraAppAcquisitionMode.PreciseQuality:
                            //    case CameraAppAcquisitionMode.PreciseQualityCont:
                            //    case CameraAppAcquisitionMode.BurstQuality:
                            //    case CameraAppAcquisitionMode.PreciseSelfQuality:
                            //        _appStatus = CamereAppStatus.Capture;
                            //        break;
                            //    case CameraAppAcquisitionMode.Record:
                            //    case CameraAppAcquisitionMode.RecordTimelapse:
                            //        _appStatus = CamereAppStatus.Record;
                            //        break;
                            //    default:
                            //        break;
                            //}
                            UpdateVisualState();
                        }, TaskContinuationOptions.ExecuteSynchronously);
                }
            }
	    }
	

        public ICommand ConfigurationCommand
        {
            get
            {
                if (_configurationCommand == null)
                {
                    _configurationCommand = new DelegateCommand(ConfigurationCommandExecute, ConfigurationCommandCanExecute);
                }

                return _configurationCommand;
            }
        }

        public ICommand StartAcquisitionCommand
        {
            get
            {
                if (_startAcquisitionCommand == null)
                {
                    _startAcquisitionCommand = new DelegateCommand(StartAcquisitionCommandExecute, StartAcquisitionCommandCanExecute);
                }
                return _startAcquisitionCommand;
            }
        }

        public ICommand StopAcquisitionCommand
        {
            get
            {
                if (_stopAcquisitionCommand == null)
                {
                    _stopAcquisitionCommand = new DelegateCommand(StopAcquisitionCommandExecute, StopAcquisitionCommandCanExecute);
                }
                return _stopAcquisitionCommand;
            }
        }



        public MainViewModel(INavigationService navigationService, IEventAggregator eventAggregator, 
            ICaneraStateRepository cameraStateRepository, ICameraAcquisitionService cameraAcquisitionService)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _cameraStateRepository = cameraStateRepository;
            _cameraAcquisitionService = cameraAcquisitionService;
            BatteryViewModel = new ViewModels.BatteryViewModel(eventAggregator, cameraStateRepository);
            //_eventAggregator.GetEvent<BatteryStateChangedEvent>().Subscribe(UpdateBattery);
        }



        #region Command methods
        private void ConfigurationCommandExecute()
        { 
            //TODO
            _navigationService.Navigate(typeof(ConfigurationViewModel));
        }

        private Boolean ConfigurationCommandCanExecute()
        {
            //TODO
            return true;
        }

        private bool StopAcquisitionCommandCanExecute()
        {
            //TODO 
            return true;
        }

        private async void StopAcquisitionCommandExecute()
        {
            if (_selectedAcquisitionMode.GetSystemMode() == CameraSystemMode.Record)
            {
                var res = await _cameraAcquisitionService.StopVideoRecord();
                _appStatus = CamereAppStatus.Vf;
                UpdateVisualState();
            }
        }

        private bool StartAcquisitionCommandCanExecute()
        {
            //TODO
            return true;
        }

        private async void StartAcquisitionCommandExecute()
        {
            if (_selectedAcquisitionMode.GetSystemMode() == CameraSystemMode.Record)
            {
                //await _cameraAcquisitionService.StartVideoRecord().ContinueWith((task) =>
                //   {
                //       _appStatus = CamereAppStatus.Recording;
                //       UpdateVisualState();
                //   }, TaskContinuationOptions.ExecuteSynchronously);

                var res = await _cameraAcquisitionService.StartVideoRecord();
                if (res.Success)
                {
                    _appStatus = CamereAppStatus.Recording;
                    UpdateVisualState();
                }
                else
                {
                    //TODO show error
                }
            }
            else
            {
                var res = await _cameraAcquisitionService.CapturePhoto(_selectedAcquisitionMode.GetCaptureMode());
                if (res.Success)
                {
                    //Todo
                    //_appStatus = CamereAppStatus.;
                    //UpdateVisualState();
                }
                else
                {
                    //TODO show error
                }
            }
        }
        #endregion

        public void OnNavigatedTo(object navigationParameter, System.Windows.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // throw new NotImplementedException();
           
            if (navigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                CameraState camState = _cameraStateRepository.GetCurrentCameraState();
                AvailableAcquisitionMode = Helpers.EnumToList<CameraAppAcquisitionMode>();
                _appStatus = camState.AppStatus;
                _selectedAcquisitionMode = camState.AppAcquisitionMode;
                UpdateVisualState();

                _eventAggregator.GetEvent<CameraSystemModeChangedEvent>().Subscribe((mode) =>
                {
                   UpdateSelectedAcquisitionMode( _cameraStateRepository.GetCurrentCameraState().AppAcquisitionMode);
                   UpdateVisualState();
                }, ThreadOption.UIThread);
            }            
        }

        public void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            //throw new NotImplementedException();
        }

        private void UpdateSelectedAcquisitionMode(CameraAppAcquisitionMode mode)
        {
            _selectedAcquisitionMode = mode;
            OnPropertyChanged("SelectedAcquisitionMode");
        }

        private void UpdateVisualState()
        {
            VisualStates vState = _selectedAcquisitionMode.GetSystemMode() == CameraSystemMode.Record ?
                VisualStates.RecordState : VisualStates.CaptureState;
            switch (_appStatus)
            {
                //case CamereAppStatus.Idle:
                //case CamereAppStatus.Vf:
                //    break;
                //case CamereAppStatus.Record:
                //    vState = VisualStates.RecordState;
                //    break;
                case CamereAppStatus.Recording:
                    vState = VisualStates.RecordingState;
                    break;
                //case CamereAppStatus.Capture:
                //    vState = VisualStates.CaptureState;
                //    break;
                case CamereAppStatus.PreciseContCapturing:
                case CamereAppStatus.BurstCapturing:
                case CamereAppStatus.PreciseCapturing:
                    vState = VisualStates.CapturingState;
                    break;
                //case CamereAppStatus.OperationDone:
                //    break;
                default:
                    break;
            }

            VisualState = vState.ToString();
        }

        #region Helper
        private enum VisualStates
        {
            RecordState,
            CaptureState,
            RecordingState,
            CapturingState,
        }
        #endregion
    }
}
