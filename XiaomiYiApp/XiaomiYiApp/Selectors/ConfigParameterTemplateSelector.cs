using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XiaomiYiApp.ViewModels;

namespace XiaomiYiApp.Selectors
{
    public class ConfigParameterTemplateSelector : TemplateSelector
    {
        public DataTemplate MultiValueItemTemplate { set; get; }

        public DataTemplate StringValueItemTemplate { set; get; }

        public DataTemplate DatetimeValueItemTemplate { set; get; }

        public DataTemplate BooleanValueItemTemplate { set; get; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ConfigurationParameterViewModel paramVm = item as ConfigurationParameterViewModel;
            DataTemplate template;
            switch (paramVm.SourceDetail.DataType)
            {
                case XiaomiYiApp.Model.Enums.ConfigurationParameteDataType.String:
                    template = StringValueItemTemplate;
                    break;
                case XiaomiYiApp.Model.Enums.ConfigurationParameteDataType.MultiValue:
                    template = MultiValueItemTemplate;
                    break;
                case XiaomiYiApp.Model.Enums.ConfigurationParameteDataType.DateTime:
                    template = DatetimeValueItemTemplate;
                    break;
                case XiaomiYiApp.Model.Enums.ConfigurationParameteDataType.Boolean:
                    template = BooleanValueItemTemplate;
                    break;
                default:
                    template = null;
                    break;
            }

            return template;
        }
    }
}
