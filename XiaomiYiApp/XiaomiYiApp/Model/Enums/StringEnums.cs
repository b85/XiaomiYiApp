using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiYiApp.Model.Enums
{

        static class NotificationMessageType
        {
            public const String BATTERY = "battery";
            public const String VIDEO_RECORD_COMPLETE = "video_record_complete";
            public const String START_VIDEO_RECORD = "start_video_record";
            public const String SWITCH_TO_CAPTURE_MODE = "switch_to_cap_mode";
            public const String SWITCH_TO_RECORD_MODE = "switch_to_rec_mode";
            public const String START_PHOTO_CAPTURE = "start_photo_capture";
            public const String PRECISE_CONT_COMPLETE = "precise_cont_complete";
            public const String PHOTO_TAKEN = "photo_taken";
            public const String BURST_COMPLETE = "burst_complete";
            public const String VF_STOP = "vf_stop";
        }

        static class ConfigurationParameterBooleanValue
        {
            public const String ON = "on";
            public const String OFF = "off";
        }

        static class SdCardInfoMessageType
        {
            public const String FREE_SPACE = "free";
            public const String TOTAL_SPACE = "total";
        }

}
