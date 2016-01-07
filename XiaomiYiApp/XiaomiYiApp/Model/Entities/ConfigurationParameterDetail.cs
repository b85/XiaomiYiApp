using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiaomiYiApp.Model.Enums;

namespace XiaomiYiApp.Model.Entities
{
    public class ConfigurationParameterDetail
    {
        public String Name { get; set; }
        public String Value { get; set; }
        public List<String> AvailableValues { set; get; }
        public Boolean IsReadOnly { get; set; }
        public ConfigurationParameteDataType DataType { set; get; } 
 
    }
}
