using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Enums;
using XiaomiYiApp.Model.Events;
using XiaomiYiApp.Model.Messages;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.Servicies
{
    public class CameraNotificationService : ICameraNotificationService
    {
        private ICameraConnectionService _connectionService;
        private IEventAggregator _eventAggregator;

        public CameraNotificationService(ICameraConnectionService connectionService, IEventAggregator eventAggregator)
        {
            _connectionService = connectionService;
            _eventAggregator = eventAggregator;

            _connectionService.UnhandledMessageReceived += _connectionService_UnhandledMessageReceived;
        }

        private void ManageNotificationMessage(RawResponseMessage message)
        {
            NotificationMessage notificationMessage;
            notificationMessage = JsonConvert.DeserializeObject<NotificationMessage>(message.JsonMessage);
            //JObject jObject = JObject.Parse(message.JsonMessage);
            //notificationMessage = jObject.ToObject<NotificationMessage>();
            switch (notificationMessage.NotificationType)
            {
                case NotificationMessageType.BATTERY:
                    ManageBatteryNotification(notificationMessage);
                    break;
                case NotificationMessageType.SWITCH_TO_CAPTURE_MODE:
                    RaiseSystemModeChanged(CameraSystemMode.Capture);
                    break;
                case NotificationMessageType.SWITCH_TO_RECORD_MODE:
                    RaiseSystemModeChanged(CameraSystemMode.Record);
                    break;
                case NotificationMessageType.VF_STOP:
                    RaiseAppStatusChanged(CamereAppStatus.Vf);
                    break;
                case NotificationMessageType.START_PHOTO_CAPTURE:
                    ManageStartPhotoCaptureNotification(notificationMessage);
                    break;
                case NotificationMessageType.START_VIDEO_RECORD:
                    RaiseAppStatusChanged(CamereAppStatus.Recording);
                    break;
                case NotificationMessageType.PHOTO_TAKEN:
                case NotificationMessageType.BURST_COMPLETE:
                case NotificationMessageType.PRECISE_CONT_COMPLETE:
                case NotificationMessageType.VIDEO_RECORD_COMPLETE:
                    ManageAcquisitionCompletedNotification(notificationMessage);
                    break;
                default:
                    break;
            }
        }

        private void ManageStartPhotoCaptureNotification(NotificationMessage message)
        { 
            //TODO
        }

        private void ManageAcquisitionCompletedNotification(NotificationMessage message)
        {
            //TODO
        }

        private void ManageBatteryNotification(NotificationMessage message)
        {
            int level = 0;
            BatteryStatus status = int.TryParse(message.Value, out level) ?
                BatteryStatus.InUse : BatteryStatus.Unknow;
            RaiseBatteryStateChanged(new BatteryInfo { BatteryLevel = level, BatteryStatus = status }); 
            
        }

        //private void ManageSystemModeNotification(NotificationMessage message)
        //{
 
        //}

        private void RaiseSystemModeChanged(CameraSystemMode mode)
        {
            _eventAggregator.GetEvent<CameraSystemModeChangedEvent>().Publish(mode);
        }

        private void RaiseBatteryStateChanged(BatteryInfo info)
        {
            _eventAggregator.GetEvent<BatteryInfoChangedEvent>().Publish(info);
        }

        private void RaiseAppStatusChanged(CamereAppStatus status)
        {
            _eventAggregator.GetEvent<CameraAppStatusChangedEvent>().Publish(status);
        }

        private void _connectionService_UnhandledMessageReceived(object sender, Model.Events.UnhandledMessageEventArgs e)
        {
            if (e.Message.MessageId == (int)MessageTypeId.Notification)
            {
                ManageNotificationMessage(e.Message);
            }
           
        }
    }
}
