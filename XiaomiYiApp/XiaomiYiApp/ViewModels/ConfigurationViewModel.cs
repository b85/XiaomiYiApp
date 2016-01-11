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
    public class ConfigurationViewModel : BindableBase
    {
        private INavigationService _navigationService;
        private IMsgBoxService _msgBoxService;
        //private ICameraConfigurationService _configurationService;
        private ICameraConfigurationRepository _cameraConfigurationRepository;

        private List<ConfigurationParameterViewModel> _parameters;    
        private DelegateCommand _saveCommand;

        public ObservableCollection<ConfigurationParameterViewModel> VideoParameters { get; private set; }
        public ObservableCollection<ConfigurationParameterViewModel> PhotoParameters { get; private set; }
        public ObservableCollection<ConfigurationParameterViewModel> SystemParameters { get; private set; }

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
            if (_parameters.Count > 0)
            {
                return;
            }

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
            }
            else
            {
                _msgBoxService.ShowNotification("Unable to read camera configuration.", "Error");
            }
        }

        private async void SaveCommandExecute()
        {
            foreach (var configParam in _parameters.Where(x=>x.IsModified))
            {
                var opResult = await _cameraConfigurationRepository.UpdateConfigurationParameterAsync(configParam.Name, configParam.CurrentValue);
                if (!opResult.Success)
                {
                    _msgBoxService.ShowNotification(String.Format("Unable to update the parameter {0}.", configParam.Name), "Error");
                    break;
                }
            }
        }

        private Boolean SaveCommandCanExecute()
        {
            return true;
        }
    }
}
