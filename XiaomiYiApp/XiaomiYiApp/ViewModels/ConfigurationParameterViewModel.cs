using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Entities;

namespace XiaomiYiApp.ViewModels
{
    public class ConfigurationParameterViewModel : BindableBase
    {
        private String _currentValue;

        public ConfigurationParameterDetail SourceDetail { get; private set; }

        public String CurrentValue 
        {
            set
            {
                if (_currentValue != value)
                {
                    _currentValue = value;
                    OnPropertyChanged("CurrentValue");
                }
            }
            get
            {
                return _currentValue;
            }
        }

        public String Name
        {
            get
            {
                return SourceDetail.Name;
            }
        }

        public Boolean IsModified
        {
            get
            {
                return String.Equals(_currentValue, SourceDetail.Value, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public ConfigurationParameterViewModel(ConfigurationParameterDetail sourceDetail)
        {
            SourceDetail = sourceDetail;
            CurrentValue = sourceDetail.Value;
        }
    }
}
