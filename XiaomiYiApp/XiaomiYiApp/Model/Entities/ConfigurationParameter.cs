using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XiaomiYiApp.Model.Entities
{
    public class ConfigurationParameter
    {
        public String Name { get; set; }
        public String Value { get; set; }

        public ConfigurationParameter(String name, String value)
        {
            Name = name;
            Value = value;
        }
    }
}
