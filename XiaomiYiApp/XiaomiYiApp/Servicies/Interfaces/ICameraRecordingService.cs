using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Enums;

namespace XiaomiYiApp.Servicies.Interfaces
{
    public interface ICameraAcquisitionService
    {
        Task<OperationResult> StartVideoRecord();

        Task<OperationResult> StopVideoRecord();

        Task<OperationResult> CapturePhoto(CameraCaptureMode currentMode);
    }
}
