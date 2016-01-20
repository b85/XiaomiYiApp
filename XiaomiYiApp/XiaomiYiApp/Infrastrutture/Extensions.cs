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


        public static CameraSystemMode GetSystemMode(this CameraAppAcquisitionMode recordingMode)
        {
            CameraSystemMode mode;
            switch (recordingMode)
            {
                case CameraAppAcquisitionMode.PreciseQuality:
                case CameraAppAcquisitionMode.PreciseQualityCont:
                case CameraAppAcquisitionMode.BurstQuality:
                case CameraAppAcquisitionMode.PreciseSelfQuality:
                    mode = CameraSystemMode.Capture;
                    break;
                case CameraAppAcquisitionMode.Record:
                case CameraAppAcquisitionMode.RecordTimelapse:
                    mode = CameraSystemMode.Record;
                    break;
                default:
                    throw new Exception("Unhandled CameraRecordingMode");
                    //break;
            }

            return mode;
        }


        public static CameraCaptureMode GetCaptureMode(this CameraAppAcquisitionMode recordingMode)
        {
            return   ((CameraCaptureMode)(int)recordingMode);
        }

        public static CameraRecordMode GetRecordMode(this CameraAppAcquisitionMode recordingMode)
        {
            return ((CameraRecordMode)(int)recordingMode);
        }


        public static CameraAppAcquisitionMode GetAppAcquisitionMode(this CameraRecordMode videoMode)
        {
            return ((CameraAppAcquisitionMode)(int)videoMode);
        }

        public static CameraAppAcquisitionMode GetAppAcquisitionMode(this CameraCaptureMode captureMode)
        {
            return ((CameraAppAcquisitionMode)(int)captureMode);
        }
    }
}
