using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using PrismClone.StoreApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking.Connectivity;
using XiaomiYiApp.Repositories.Interfaces;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.ViewModels
{
    class ConnectViewModel : BindableBase
    {
        private ICameraConnectionService _connectionService;
        private INavigationService _navigationService;
        private ICameraConfigurationRepository _configurationRepository;

        private DelegateCommand _connectCommand;
        private String _statusMessage;
        private String _visualState;



        public String VisualState
        {
            get { return _visualState; }
            private set 
            { 
                _visualState = value;
                OnPropertyChanged("VisualState");
            }
        }
        

        public String StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged("StatusMessage");
                }
            }
        }

        public ICommand ConnectCommand
        {
            get {
                    if (_connectCommand == null)
                    {
                        _connectCommand = new DelegateCommand(ConnectCommandExecute, ConnectCommandCanExecute);
                    }

                    return _connectCommand;
            }
        }

        public ConnectViewModel(ICameraConnectionService connectionService, INavigationService navigationService, ICameraConfigurationRepository configurationRepository)
        {
            _connectionService = connectionService;
            _navigationService = navigationService;
            _configurationRepository = configurationRepository;
            _visualState = VisualStates.Disconnected.ToString();
        }

        private async void ConnectCommandExecute()
        {

            if (IsWifiConnected())
            {
                VisualState = VisualStates.Connecting.ToString();
                var result = await _connectionService.ConnenctDataAsync();
                await Task.Delay(3000);
                if (result.Success)
                {

                    result = await _configurationRepository.LoadDetailedConfigurationAsync();
                    if (result.Success)
                    {
                        //VisualState = VisualStates.Connected.ToString();
                        _navigationService.Navigate(typeof(MainViewModel));
                        VisualState = VisualStates.Connected.ToString();
                        return;
                    }
                }
              
                //Fail
                StatusMessage = result.ResultMessage;
                VisualState = VisualStates.Disconnected.ToString();
                
            }
        }

        private Boolean ConnectCommandCanExecute()
        {
            //TODO
            return true;
        }


        private Boolean IsWifiConnected()
        {
            Boolean result = true;
            if (DeviceNetworkInformation.IsWiFiEnabled)
            {
                ConnectionProfile connProfile = NetworkInformation.GetInternetConnectionProfile();
                //.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.LocalAccess
                if (connProfile == null)
                {
                    //TODO message;
                    StatusMessage = "Wifi disconnected";
                    result = false;
                }
            }
            else
            {
                //TODO message;
                StatusMessage = "Wifi disabled";
                result = false;
            }

            return result;
        }


        #region Private
        private enum VisualStates
        {
            Disconnected,
            Connecting,
            Connected,
        }
        #endregion

    }
}
