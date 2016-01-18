using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using XiaomiYiApp.Model.Enums;

namespace XiaomiYiApp.Model.Messages
{
    public class RequestMessage : Message
    {
        [JsonProperty("token")]
        public int Token { set; get; }

        public RequestMessage()
        { 
        }

        public RequestMessage(MessageTypeId messageTypeId)
        {
            MessageId = (int)messageTypeId;
        }
    }
}
