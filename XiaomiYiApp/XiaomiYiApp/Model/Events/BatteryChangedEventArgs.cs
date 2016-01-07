using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiaomiYiApp.Model.Enums;

namespace XiaomiYiApp.Model.Events
{
    class BatteryChangedEventArgs : EventArgs
    {
        public int BatteryLevel { get; set; }
        public BatteryStatus BatteryStatus { get; set; }
    }
}
