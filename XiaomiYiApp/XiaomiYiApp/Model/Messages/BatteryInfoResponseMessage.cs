using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiYiApp.Model.Messages
{
    public class BatteryInfoResponseMessage : BaseResponseMessage
    {
        [JsonProperty("param")]
        public int BatteryLevel { set; get; }

        [JsonProperty("type")]
        public String BatteryMode { set; get; }
    }
}
