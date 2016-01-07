using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Messages;

namespace XiaomiYiApp.Infrastrutture
{
    public class RequestStateObjectBase
    {
        //public EventWaitHandle CompletedWaitHandle { set; get; }

        public int MessageId { set; get; }

        //public RawResponseMessage ResponseMessage { set; get; }
    }

    public class RequestStateObject : RequestStateObjectBase
    {
        public EventWaitHandle CompletedWaitHandle { set; get; }


        public RawResponseMessage ResponseMessage { set; get; }
    }

    public class RequestStateObjectAsync : RequestStateObjectBase
    {
        public TaskCompletionSource<RawResponseMessage> ResponseTaskCompletionSource { set; get; }

        //public RequestStateObjectAsync()
        //{
        //    ResponseTaskCompletionSource = new TaskCompletionSource<RawResponseMessage>();
        //}
    }
}
