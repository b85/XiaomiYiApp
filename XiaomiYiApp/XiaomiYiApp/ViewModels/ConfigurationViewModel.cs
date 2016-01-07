using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
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

        public List<ConfigurationParameterViewModel> VideoParameters { get; private set; }
        public List<ConfigurationParameterViewModel> PhotoParameters { get; private set; }
        public List<ConfigurationParameterViewModel> SystemParameters { get; private set; }

        public ConfigurationViewModel(INavigationService navigationService, ICameraConfigurationRepository cameraConfigurationRepository, IMsgBoxService msgBoxService)
        {
            _navigationService = navigationService;
            _msgBoxService = msgBoxService;
            _cameraConfigurationRepository = cameraConfigurationRepository;
            Parameters = new List<ConfigurationParameterViewModel>();
            VideoParameters = new List<ConfigurationParameterViewModel>();
            PhotoParameters = new List<ConfigurationParameterViewModel>();
            SystemParameters = new List<ConfigurationParameterViewModel>();
           // _configurationService = configurationService;

            LoadDetailedConfigurationAsync();
            
        }

        private async void LoadDetailedConfigurationAsync()
        {
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
            }
            else
            {
                _msgBoxService.ShowNotification("Configuration readind failed." , "Error");
            }
        }
    }
}
