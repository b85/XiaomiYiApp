using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace XiaomiYiApp.Model.Messages
{
    public class NotificationMessage : Message
    {
        [JsonProperty("type")]
        public String NotificationType { set; get; }

        [JsonProperty("param")]
        public String Value { set; get; }
    }
}
