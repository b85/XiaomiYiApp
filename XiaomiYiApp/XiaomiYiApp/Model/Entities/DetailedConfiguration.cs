using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XiaomiYiApp.Model.Entities
{
    public class DetailedConfiguration
    {
        public List<ConfigurationParameterDetail> Parameters { get; set; }

        public ConfigurationParameterDetail this [String parameterName]
        {
            get
            {
               return Parameters.FirstOrDefault(x => String.Equals(x.Name, parameterName, StringComparison.CurrentCultureIgnoreCase));
            }
        }
    }
}
