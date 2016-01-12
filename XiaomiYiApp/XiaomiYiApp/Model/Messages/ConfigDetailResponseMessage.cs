using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace XiaomiYiApp.Model.Messages
{
    public class ConfigDetailResponseMessage : BaseResponseMessage
    {
        [JsonProperty("param")]
        public List<Dictionary<String, String>> RawDetail { set; get; }

        [JsonIgnore]
        public String ParameterName
        {
            get
            {
                return RawDetail != null ? RawDetail.First().First().Key : (String)null;
            }
        }

        [JsonIgnore]
        public String Detail
        {
            get
            {
                return RawDetail != null ? RawDetail.First().First().Value : (String)null;
            }
        }
    }
}
