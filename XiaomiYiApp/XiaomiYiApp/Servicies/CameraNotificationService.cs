using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.Servicies
{
    public class CameraNotificationService : ICameraNotificationService
    {
        private ICameraConnectionService _connectionService;

        public CameraNotificationService(ICameraConnectionService connectionService)
        {
            _connectionService = connectionService;
        }
    }
}
