using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XiaomiYiApp.Model.Messages
{
    public class RawResponseMessage
    {
        public int MessageId { set; get; }
        public String JsonMessage { set; get; }

        public override string ToString()
        {
            return JsonMessage;
        }
    }
}
