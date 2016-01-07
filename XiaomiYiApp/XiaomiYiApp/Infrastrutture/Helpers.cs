using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Enums;
using XiaomiYiApp.Model.Messages;

namespace XiaomiYiApp.Infrastrutture
{
    class Helpers
    {
        private static String SETTABLE_PARAMETER_IDENTIFIER = "settable:";
        private static String READONLY_PARAMETER_IDENTIFIER = "readonly:";
        private static char AVAILABLE_VALUES_SEPARATOR = '#';


        public static ConfigurationParameterDetail GetParameterDetailFromMessage(ConfigDetailResponseMessage message)
        {
            ConfigurationParameterDetail paramDetail = null;
            if (message.Success)
            {
                paramDetail = new ConfigurationParameterDetail();
                String valuesStr = String.Empty;
                paramDetail.Name = message.ParameterName;
                if (message.Detail.StartsWith(SETTABLE_PARAMETER_IDENTIFIER))
                {
                    valuesStr = message.Detail.Remove(0, SETTABLE_PARAMETER_IDENTIFIER.Length);
                    paramDetail.IsReadOnly = false;
                }
                else if (message.Detail.StartsWith(READONLY_PARAMETER_IDENTIFIER))
                {
                    valuesStr = message.Detail.Remove(0, READONLY_PARAMETER_IDENTIFIER.Length);
                    paramDetail.IsReadOnly = true;
                }

                paramDetail.AvailableValues = valuesStr.Split(AVAILABLE_VALUES_SEPARATOR).ToList();

                if (paramDetail.AvailableValues.Count <= 1)
                {
                    if (paramDetail.Name == ConfigurationParameterName.CAMERA_CLOCK)
                    {
                        paramDetail.DataType = Model.Enums.ConfigurationParameteDataType.DateTime;
                    }
                    else
                    {
                        paramDetail.DataType = Model.Enums.ConfigurationParameteDataType.String;
                    }
                }
                else if (paramDetail.AvailableValues.Count == 2 
                    && paramDetail.AvailableValues.Contains("off") && paramDetail.AvailableValues.Contains("on"))
                {
                    paramDetail.DataType = Model.Enums.ConfigurationParameteDataType.Boolean;
                }
                else
                {
                    paramDetail.DataType = Model.Enums.ConfigurationParameteDataType.MultiValue;
                }

            }

            return paramDetail;
        }

        public static List<ConfigurationParameter> GetConfigurationFromMessage(FullConfigResponseMessage message)
        {
            if (message.Success)
            {
                return message.Configuration.Select(x => new ConfigurationParameter(x.First().Key, x.First().Value)).ToList();
            }
            return null;
        }

        public static Boolean IsVideoConfigurationParameter(String parameterName)
        {
            switch (parameterName.ToLower())
            {
                case ConfigurationParameterName.TIMELAPSE_VIDEO:
                case ConfigurationParameterName.TIMELAPSE_VIDEO_DURATION:
                case ConfigurationParameterName.TIMELAPSE_VIDEO_RESOLUTION:
                case ConfigurationParameterName.VIDEO_QUALITY:
                case ConfigurationParameterName.VIDEO_RESOLUTION:
                case ConfigurationParameterName.VIDEO_ROTATE:
                case ConfigurationParameterName.VIDEO_STAMP:
                case ConfigurationParameterName.VIDEO_STANDARD:
                    return true;
                default:
                    return false;
            }
        }

        public static Boolean IsPhotoConfigurationParameter(String parameterName)
        {
            switch (parameterName.ToLower())
            {
                case ConfigurationParameterName.BURST_CAPTURE_NUMBER:
                case ConfigurationParameterName.PHOTO_QUALITY:
                case ConfigurationParameterName.PHOTO_SIZE:
                case ConfigurationParameterName.PHOTO_STAMP:
                case ConfigurationParameterName.PRECISE_CONT_CAPTURING:
                case ConfigurationParameterName.PRECISE_CONT_TIME:
                case ConfigurationParameterName.PRECISE_SELF_REMAIN_TIME:
                case ConfigurationParameterName.PRECISE_SELF_RUNNING:
                case ConfigurationParameterName.PRECISE_SELFTIME:

                case ConfigurationParameterName.RECORD_PHOTO_TIME:
                case ConfigurationParameterName.TIMELAPSE_PHOTO:
                    return true;
                default:
                    return false;
            }
        }

        public static Boolean IsSystemConfigurationParameter(String parameterName)
        {
            return !IsPhotoConfigurationParameter(parameterName) && !IsVideoConfigurationParameter(parameterName);
        }
    }
}
