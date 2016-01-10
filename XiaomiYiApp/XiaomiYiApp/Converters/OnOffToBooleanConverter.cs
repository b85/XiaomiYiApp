using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XiaomiYiApp.Model.Enums;

namespace XiaomiYiApp.Converters
{
    public class OnOffToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool) &&  Nullable.GetUnderlyingType (targetType) !=  typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return (value != null && String.Equals(value.ToString(), ConfigurationParameterBooleanValue.ON, StringComparison.CurrentCultureIgnoreCase));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a string");

            if (!(value is Boolean))
                throw new InvalidOperationException("Value must be a boolean");

            return (Boolean)value ? ConfigurationParameterBooleanValue.ON : ConfigurationParameterBooleanValue.OFF;
        }
    }
}
