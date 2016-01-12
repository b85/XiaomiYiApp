using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiYiApp.Model.Messages
{
    public class ResponseMessageWithParam : BaseResponseMessage
    {
        [JsonProperty("param")]
        public String Param { set; get; }
    }
}
