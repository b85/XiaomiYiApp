﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Enums;

namespace XiaomiYiApp.Repositories.Interfaces
{
    public interface ICaneraStateRepository
    {
        Boolean IsLoaded { get; }

        Task<OperationResult> LoadCameraStateAsync();

        CameraState GetCurrentCameraState();

        Task<OperationResult> SetSystemModeAsync(CameraSystemMode systemMode);
        
        Task<OperationResult> SetRecordModeAsync(CameraRecordMode recordMode);
       
        Task<OperationResult> SetCaptureModeAsync(CameraCaptureMode captureMode);

        Task<OperationResult> SetAppAcquisitionMode(CameraAppAcquisitionMode acquisitionMode);
    
    }
}
