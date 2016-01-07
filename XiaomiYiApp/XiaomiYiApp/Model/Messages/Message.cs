using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace XiaomiYiApp.Model.Messages
{
    public abstract class Message
    {
        [JsonProperty("msg_id")]
        public int MessageId { set; get; }
        
    }
}
