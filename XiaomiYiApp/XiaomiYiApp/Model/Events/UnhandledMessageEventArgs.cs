using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiaomiYiApp.Model.Messages;

namespace XiaomiYiApp.Model.Events
{
    public class UnhandledMessageEventArgs : EventArgs
    {
        public RawResponseMessage Message { set; get; }
    }
}
