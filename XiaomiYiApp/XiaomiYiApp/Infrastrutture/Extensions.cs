using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Enums;

namespace XiaomiYiApp.Infrastrutture
{
    public static class Extensions
    {
        public static T GetEnumFromDescription<T>(this String description) //where T : Enum
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Enum description not found.", "description");
            // or return default(T);
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }


        public static CameraSystemMode GetSystemMode(this CameraRecordingMode recordingMode)
        {
            CameraSystemMode mode;
            switch (recordingMode)
            {
                case CameraRecordingMode.PreciseQuality:
                case CameraRecordingMode.PreciseQualityCont:
                case CameraRecordingMode.BurstQuality:
                case CameraRecordingMode.PreciseSelfQuality:
                    mode = CameraSystemMode.Capture;
                    break;
                case CameraRecordingMode.Record:
                case CameraRecordingMode.RecordTimelapse:
                    mode = CameraSystemMode.Video;
                    break;
                default:
                    throw new Exception("Unhandled CameraRecordingMode");
                    //break;
            }

            return mode;
        }


        public static CameraCaptureMode GetCaptureMode(this CameraRecordingMode recordingMode)
        {
            return ((CameraCaptureMode)(int)recordingMode);
        }

        public static CameraVideoMode GetVideoMode(this CameraRecordingMode recordingMode)
        {
            return ((CameraVideoMode)(int)recordingMode);
        }


        public static CameraRecordingMode GetRecordingMode(this CameraVideoMode videoMode)
        {
            return ((CameraRecordingMode)(int)videoMode);
        }

        public static CameraRecordingMode GetRecordingMode(this CameraCaptureMode captureMode)
        {
            return ((CameraRecordingMode)(int)captureMode);
        }
    }
}
