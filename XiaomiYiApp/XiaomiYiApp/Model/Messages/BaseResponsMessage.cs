using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace XiaomiYiApp.Model.Messages
{
    public class BaseResponsMessage : Message
    {
        [JsonProperty("rval")]
        public int Result { get; set; }

        [JsonIgnore]
        public bool Success
        {
            get { return Result >= 0; }
        }
    }
}
