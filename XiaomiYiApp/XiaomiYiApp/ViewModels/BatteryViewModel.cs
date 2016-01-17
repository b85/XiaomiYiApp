using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Events;
using XiaomiYiApp.Repositories.Interfaces;

namespace XiaomiYiApp.ViewModels
{
    public class BatteryViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private int _batteryLevel;

        public int BatteryLevel
        {
            get { return _batteryLevel; }
            set 
            {
                if (_batteryLevel != value)
                {
                    _batteryLevel = value;
                    OnPropertyChanged("BatteryLevel");
                }
            }
        }

        public BatteryViewModel(IEventAggregator eventAggregator, ICaneraStateRepository cameraStateRepository)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<BatteryInfoChangedEvent>().Subscribe((e) => { BatteryLevel = e.BatteryLevel; }, ThreadOption.UIThread);
            _batteryLevel = cameraStateRepository.GetCurrentCameraState().Battery.BatteryLevel;
        }
    }
}
