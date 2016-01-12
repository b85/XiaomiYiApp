using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Enums;

namespace XiaomiYiApp.Model.Entities
{
    public class CameraState
    {
        public CameraSystemMode SystemMode { get; set; }
        public CameraCaptureMode CaptureMode { get; set; }
        public CameraVideoMode VideoMode { get; set; }

        public BatteryInfo Battery { get; set; }
        public SdCardInfo Storage { get; set; }
    }
}
