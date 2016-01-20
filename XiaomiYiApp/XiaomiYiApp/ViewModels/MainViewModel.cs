﻿using Microsoft.Practices.Prism.Commands;
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
                            OnPropertyChanged("SelectedRecordingMode");
                            UpdateVisualState();
                        });
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

        private void StopAcquisitionCommandExecute()
        {
            throw new NotImplementedException();
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
                var res = await _cameraAcquisitionService.StartVideoRecord();
                _appStatus = CamereAppStatus.Recording;
                UpdateVisualState();
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
                SelectedAcquisitionMode = camState.AppAcquisitionMode;
                _appStatus = camState.AppStatus;
                UpdateVisualState();
            }
            
            
        }

        public void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            //throw new NotImplementedException();
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
                case CamereAppStatus.Record:
                    vState = VisualStates.RecordState;
                    break;
                case CamereAppStatus.Recording:
                    vState = VisualStates.RecordingState;
                    break;
                case CamereAppStatus.Capture:
                    vState = VisualStates.CaptureState;
                    break;
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
