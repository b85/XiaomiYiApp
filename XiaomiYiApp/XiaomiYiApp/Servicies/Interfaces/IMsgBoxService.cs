using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiYiApp.Servicies.Interfaces
{
    public interface IMsgBoxService
    {

        void ShowNotification(string message, String caption );    
        bool AskForConfirmation(string message);
    }
}
