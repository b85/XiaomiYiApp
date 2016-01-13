using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XiaomiYiApp.Model.Entities
{
    public class Configuration
    {
        public List<ConfigurationParameter> Parameters { get; set; }


        public String GetValue(String parameterName)
        {
            return Parameters.Any(x => String.Equals(x.Name, parameterName, StringComparison.CurrentCultureIgnoreCase)) ?
                Parameters.First(x => String.Equals(x.Name, parameterName, StringComparison.CurrentCultureIgnoreCase)).Value : null;
        }
        //public String this[String parameterName]
        //{
        //    get
        //    {
        //        return Parameters.Any(x=>x.
        //    }
        //}
    }
}
