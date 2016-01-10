using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public List<ConfigurationParameterViewModel> Parameters { get; private set; }

        public ObservableCollection<ConfigurationParameterViewModel> VideoParameters { get; private set; }
        public ObservableCollection<ConfigurationParameterViewModel> PhotoParameters { get; private set; }
        public ObservableCollection<ConfigurationParameterViewModel> SystemParameters { get; private set; }

        public ConfigurationViewModel(INavigationService navigationService, ICameraConfigurationRepository cameraConfigurationRepository, IMsgBoxService msgBoxService)
        {
            _navigationService = navigationService;
            _msgBoxService = msgBoxService;
            _cameraConfigurationRepository = cameraConfigurationRepository;
            Parameters = new List<ConfigurationParameterViewModel>();
            VideoParameters = new ObservableCollection<ConfigurationParameterViewModel>();
            PhotoParameters = new ObservableCollection<ConfigurationParameterViewModel>();
            SystemParameters = new ObservableCollection<ConfigurationParameterViewModel>();
           // _configurationService = configurationService;

           //LoadDetailedConfigurationAsync();
            
        }

        internal async Task LoadDetailedConfigurationAsync()
        {
            if (Parameters.Count > 0)
            {
                return;
            }

            var result = await _cameraConfigurationRepository.GetDetailedConfigurationAsync();
            if(result.Success)
            {
                ConfigurationParameterViewModel paramVm;
                foreach (var paramDetail in result.Result.Parameters)
                {
                    paramVm = Parameters.FirstOrDefault(x => x.SourceDetail.Name == paramDetail.Name);
                    if (paramVm != null)
                    {
                        paramVm.CurrentValue = paramDetail.Value;
                    }
                    else
                    {
                        paramVm = new ConfigurationParameterViewModel(paramDetail);
                        Parameters.Add(paramVm);

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
                _msgBoxService.ShowNotification("Configuration readind failed." , "Error");
            }
        }
    }
}
