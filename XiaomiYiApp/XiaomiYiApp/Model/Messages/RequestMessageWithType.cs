using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiYiApp.Model.Messages
{
    class RequestMessageWithType : RequestMessage
    {
        [JsonProperty("type")]
        public String Type { set; get; }
    }
}
