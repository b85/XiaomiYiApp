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
                default:
                    break;
            }
        }

        private void ManageBatteryNotification(NotificationMessage message)
        {
            int level = 0;
            BatteryStatus status = int.TryParse(message.Value, out level) ?
                BatteryStatus.InUse : BatteryStatus.Unknow;
            RaiseBatteryStateChanged(new BatteryState { BatteryLevel = level, BatteryStatus = status }); 
            
        }

        private void RaiseBatteryStateChanged(BatteryState state)
        {
            _eventAggregator.GetEvent<BatteryStateChangedEvent>().Publish(state);
        }

        private void _connectionService_UnhandledMessageReceived(object sender, Model.Events.UnhandledMessageEventArgs e)
        {
            if (e.Message.MessageId == (int)MessageType.Notification)
            {
                ManageNotificationMessage(e.Message);
            }
           
        }
    }
}
