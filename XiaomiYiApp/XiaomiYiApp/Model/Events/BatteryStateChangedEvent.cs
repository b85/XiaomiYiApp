using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiaomiYiApp.Model.Entities;

namespace XiaomiYiApp.Model.Events
{
    class BatteryStateChangedEvent : PubSubEvent<BatteryState>
    {
    }
}
