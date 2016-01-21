using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Enums;

namespace XiaomiYiApp.Model.Events
{
    public class CameraAppStatusChangedEvent : PubSubEvent<CamereAppStatus>
    {
    }
}
