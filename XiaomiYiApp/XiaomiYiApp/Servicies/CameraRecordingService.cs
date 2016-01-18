using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Enums;
using XiaomiYiApp.Model.Messages;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.Servicies
{
    public class CameraRecordingService : ICameraRecordingService
    {
        private ICameraConnectionService _cameraConnectionService;

        public CameraRecordingService(ICameraConnectionService cameraConnectionService)
        {
            _cameraConnectionService = cameraConnectionService;
        }
        
        public async Task<OperationResult> StartVideoRecord()
        {
            RequestMessage request = new RequestMessage (MessageTypeId.StartVideoRec);
            var res = await _cameraConnectionService.SendMessageAsync<BaseResponseMessage>(request);

            if (!res.Success)
            {
                return OperationResult.FromResult(res);
            }
            else
            {
                return new OperationResult { Success = res.Result.Success, ResultMessage = res.Result.Result.ToString(), };
            }
        }

        public async Task<OperationResult> StopVideoRecord()
        {
            RequestMessage request = new RequestMessage(MessageTypeId.StopVideoRec);
            var res = await _cameraConnectionService.SendMessageAsync<BaseResponseMessage>(request);

            if (!res.Success)
            {
                return OperationResult.FromResult(res);
            }
            else
            {
                return new OperationResult { Success = res.Result.Success, ResultMessage = res.Result.Result.ToString(), };
            }
        }

        public async Task<OperationResult> CapturePhoto(CameraCaptureMode mode)
        {
            RequestMessage request;
            if (mode == CameraCaptureMode.PreciseQualityCont)
            {
                request = new RequestMessage(MessageTypeId.CapturePhotoCont);
            }
            else
            {
                request = new RequestMessage(MessageTypeId.CapturePhoto);
            }

            var res = await _cameraConnectionService.SendMessageAsync<BaseResponseMessage>(request);

            if (!res.Success)
            {
                return OperationResult.FromResult(res);
            }
            else
            {
                return new OperationResult { Success = res.Result.Success, ResultMessage = res.Result.Result.ToString(), };
            }
        }

/*

        def ActionPhoto(self): 
677 		myid = 769 
678 		tosend = '{"msg_id":769,"token":%s}' %self.token 
679 		if self.camconfig["capture_mode"] == "precise quality cont.": 
680 			if self.camconfig["precise_cont_capturing"] == "on": 
681 				tosend = '{"msg_id":770,"token":%s}' %self.token 
682 				myid = 770 
683 			self.Comm(tosend) 
684 		else: 
685 			self.Comm(tosend) 
686 		self.ReadConfig() 
687 		self.UpdateUsage() 
688 
 
689 
 
690 	def ActionRecordStart(self): 
691 		self.UpdateUsage() 
692 		tosend = '{"msg_id":513,"token":%s}' %self.token 
693 		self.Comm(tosend) 
694 		self.brecord.config(text="STOP\nrecording", command=self.ActionRecordStop, bg="#ff6666") 
695 		self.brecord.update_idletasks() 
696 		self.bphoto.config(state=DISABLED) 
697 		self.bphoto.update_idletasks()  
698 		self.ReadConfig() 
699 
 
700 	def ActionRecordStop(self): 
701 		tosend = '{"msg_id":514,"token":%s}' %self.token 
702 		self.Comm(tosend) 
703 		self.brecord.config(text="START\nrecording", command=self.ActionRecordStart, bg="#66ff66") 
704 		self.brecord.update_idletasks() 
705 		self.bphoto.config(state="normal") 
706 		self.bphoto.update_idletasks()  
707 		self.ReadConfig() 
708 		self.UpdateUsage() 
 * */

    }
}
