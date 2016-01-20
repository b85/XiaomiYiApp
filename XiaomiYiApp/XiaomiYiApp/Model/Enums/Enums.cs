using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public enum MessageTypeId
    {
        Connect = 257,
        ConfigurationGet = 3,
        ConfigurationSet = 2,
        Notification = 7,
        SdCardInfo = 5,
        BatteryInfo = 13,
        StartVideoRec = 513,
        StopVideoRec = 514,
        CapturePhoto = 769,
        CapturePhotoCont = 770,
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
        [Description("capture")]
        Capture,  //photo
        [Description("record")]
        Record
    }

    public enum CameraRecordMode
    {
        [Description("record")]
        Record = 4,
        [Description("record_timelapse")]
        RecordTimelapse = 5
    }

    public enum CameraCaptureMode
    {
        [Description("precise quality")]
        PreciseQuality = 0,
        [Description("precise quality cont.")]
        PreciseQualityCont = 1,
        [Description("burst quality")]
        BurstQuality = 2,
        [Description("precise self quality")]
        PreciseSelfQuality = 3,

        //precise quality#precise quality cont.#burst quality#precise self quality
    }

    public enum CameraAppAcquisitionMode
    {
        PreciseQuality = 0,  
        PreciseQualityCont = 1,
        BurstQuality = 2,
        PreciseSelfQuality = 3,
        Record = 4,
        RecordTimelapse = 5
    }

    public enum CamereAppStatus
    {
        [Description("idle")]
        Idle,
        [Description("vf")]
        Vf,
        [Description("record")]
        Record,
        [Description("recording")]
        Recording,
        [Description("capture")]
        Capture,
        [Description("precise_cont_capturing")]
        PreciseContCapturing,
        [Description("burst_capturing")]
        BurstCapturing,
        [Description("precise_capturing")]
        PreciseCapturing,
        [Description("operation_done")]
        OperationDone,
    }
}
