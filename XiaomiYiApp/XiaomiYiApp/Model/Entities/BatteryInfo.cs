using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Enums;

namespace XiaomiYiApp.Model.Entities
{
    public class BatteryInfo
    {
        public int BatteryLevel { get; set; }
        public BatteryStatus BatteryStatus { get; set; }
    }
}
