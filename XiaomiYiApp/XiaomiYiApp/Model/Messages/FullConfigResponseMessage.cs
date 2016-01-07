using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace XiaomiYiApp.Model.Messages
{
    public class FullConfigResponseMessage : BaseResponsMessage
    {
        [JsonProperty("param")]
        public List<Dictionary<String,String>> Configuration { set; get; }
    }
}
