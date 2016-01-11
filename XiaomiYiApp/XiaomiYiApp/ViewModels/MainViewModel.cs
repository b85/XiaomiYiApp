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
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Events;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.ViewModels
{
    class MainViewModel : BindableBase , INavigationAware
    {
      
        private  IEventAggregator _eventAggregator;
        private INavigationService _navigationService;

        private DelegateCommand _configurationCommand;

        public BatteryViewModel BatteryViewModel { get; private set; }

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

        public MainViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            BatteryViewModel = new ViewModels.BatteryViewModel(eventAggregator);

            //_eventAggregator.GetEvent<BatteryStateChangedEvent>().Subscribe(UpdateBattery);
        }


        //private void UpdateBattery(BatteryState state)
        //{
        //    //TODO
        //}

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

        public void OnNavigatedTo(object navigationParameter, System.Windows.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // throw new NotImplementedException();
        }

        public void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            //throw new NotImplementedException();
        }
    }
}
