using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Events;

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
        
        public BatteryViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<BatteryStateChangedEvent>().Subscribe((e) => { BatteryLevel = e.BatteryLevel; }, ThreadOption.UIThread);
        }
    }
}
