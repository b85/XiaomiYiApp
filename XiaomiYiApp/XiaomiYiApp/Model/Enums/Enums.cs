using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XiaomiYiApp.Model.Enums
{
    public enum BatteryStatus
    {
        Unknow,
        InUse,
        InCharge,
    }

    public enum MessageType
    {
        Connect = 257,
        ConfigurationGet = 3,
        ConfigurationSet = 2,
        Notification = 7,
        SdCardInfo = 5,
        BatteryInfo = 13
    }

    public enum ConfigurationParameteDataType
    {
        String,
        MultiValue,
        DateTime,
        Boolean
    }

    public enum CameraSystemMode
    {
        Capture,  //photo
        Video
    }

    public enum CameraVideoMode
    {
        //TODO
    }

    public enum CameraCaptureMode
    {
        //TODO
        //precise quality#precise quality cont.#burst quality#precise self quality
    }
}
