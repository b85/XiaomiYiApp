using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace XiaomiYiApp.Model.Messages
{
    public class RequestMessageWithParam : RequestMessage
    {
        [JsonProperty("param")]
        public String Param { set; get; }
    }
}
