using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.ViewModels
{
    class MainViewModel : BindableBase
    {
        private ICameraNotificationService _noticationService;
        private INavigationService _navigationService;

        private DelegateCommand _configurationCommand;

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

        public MainViewModel(ICameraNotificationService noticationService, INavigationService navigationService)
        {
            _noticationService = noticationService;
            _navigationService = navigationService;
        }




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
    }
}
