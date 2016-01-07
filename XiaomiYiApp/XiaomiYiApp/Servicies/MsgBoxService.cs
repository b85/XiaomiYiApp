using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.Servicies
{
    public class MsgBoxService : IMsgBoxService
    {
        public void ShowNotification(string message = "", String caption = "") {
            
          MessageBox.Show(message, caption, MessageBoxButton.OK);
        }


        
        public bool AskForConfirmation(string message) { MessageBoxResult result = MessageBox.Show(message, "", MessageBoxButton.OKCancel); return result.HasFlag(MessageBoxResult.OK); }
    }
}
