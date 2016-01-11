using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using PrismClone.StoreApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XiaomiYiApp.Infrastrutture;
using XiaomiYiApp.Repositories.Interfaces;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.ViewModels
{
    public class ConfigurationViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;
        private IMsgBoxService _msgBoxService;
        //private ICameraConfigurationService _configurationService;
        private ICameraConfigurationRepository _cameraConfigurationRepository;
        
        private List<ConfigurationParameterViewModel> _parameters;    
        private DelegateCommand _saveCommand;
        private Boolean _isWaiting;

        public ObservableCollection<ConfigurationParameterViewModel> VideoParameters { get; private set; }
        public ObservableCollection<ConfigurationParameterViewModel> PhotoParameters { get; private set; }
        public ObservableCollection<ConfigurationParameterViewModel> SystemParameters { get; private set; }

        public Boolean IsWaiting 
        {
            get { return _isWaiting; }
            set
            {
                if((_isWaiting != value))
                {
                    _isWaiting = value;
                    OnPropertyChanged("IsWaiting");
                }
            }
        }

        public DelegateCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new DelegateCommand(SaveCommandExecute, SaveCommandCanExecute);
                }
                return _saveCommand;
            }
        }

        public ConfigurationViewModel(INavigationService navigationService, ICameraConfigurationRepository cameraConfigurationRepository, IMsgBoxService msgBoxService)
        {
            _navigationService = navigationService;
            _msgBoxService = msgBoxService;
            _cameraConfigurationRepository = cameraConfigurationRepository;
            _parameters = new List<ConfigurationParameterViewModel>();
            VideoParameters = new ObservableCollection<ConfigurationParameterViewModel>();
            PhotoParameters = new ObservableCollection<ConfigurationParameterViewModel>();
            SystemParameters = new ObservableCollection<ConfigurationParameterViewModel>();
           // _configurationService = configurationService;

           //LoadDetailedConfigurationAsync();
            
        }

        internal async Task LoadDetailedConfigurationAsync()
        {
            //if (_parameters.Count > 0)
            //{
            //    return;
            //}

            IsWaiting = true;
            var result = await _cameraConfigurationRepository.GetDetailedConfigurationAsync();
            if(result.Success)
            {
                ConfigurationParameterViewModel paramVm;
                foreach (var paramDetail in result.Result.Parameters)
                {
                    paramVm = _parameters.FirstOrDefault(x => x.SourceDetail.Name == paramDetail.Name);
                    if (paramVm != null)
                    {
                        paramVm.CurrentValue = paramDetail.Value;
                    }
                    else
                    {
                        paramVm = new ConfigurationParameterViewModel(paramDetail);
                        _parameters.Add(paramVm);

                        if (Helpers.IsVideoConfigurationParameter(paramDetail.Name))
                        {
                            VideoParameters.Add(paramVm);
                        }
                        else if (Helpers.IsPhotoConfigurationParameter(paramDetail.Name))
                        {
                            PhotoParameters.Add(paramVm);
                        }
                        else
                        {
                            SystemParameters.Add(paramVm);
                        }
                    }
                }
                //OnPropertyChanged("VideoParameters");
                //OnPropertyChanged("PhotoParameters");
                //OnPropertyChanged("SystemParameters");
                IsWaiting = false;
            }
            else
            {
                IsWaiting = false;
                _msgBoxService.ShowNotification("Unable to read camera configuration.", "Error");
            }
        }

        private async void SaveCommandExecute()
        {
            IsWaiting = true;
            foreach (var configParam in _parameters.Where(x=>x.IsModified))
            {
                var opResult = await _cameraConfigurationRepository.UpdateConfigurationParameterAsync(configParam.Name, configParam.CurrentValue);
                if (!opResult.Success)
                {
                    IsWaiting = false;
                    _msgBoxService.ShowNotification(String.Format("Unable to update the parameter {0}.", configParam.Name), "Error");
                    break;
                }
            }
            IsWaiting = false;
        }

        private Boolean SaveCommandCanExecute()
        {
            return !IsWaiting;
        }

        public async void OnNavigatedTo(object navigationParameter, System.Windows.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
           // throw new NotImplementedException();
            if (navigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                await LoadDetailedConfigurationAsync();
            }
        }

        public void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            //throw new NotImplementedException();
        }
    }
}
