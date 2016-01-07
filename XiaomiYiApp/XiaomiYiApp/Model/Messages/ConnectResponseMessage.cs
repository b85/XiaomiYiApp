using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace XiaomiYiApp.Model.Messages
{
    public class ConnectResponseMessage : BaseResponsMessage
    {
        [JsonProperty("param")]
        public int Param { set; get; }
       
    }
}
