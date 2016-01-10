using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Events;
using XiaomiYiApp.Model.Messages;

namespace XiaomiYiApp.Servicies.Interfaces
{
    public interface ICameraConnectionService
    {
        Boolean IsConnected { get; }

        event EventHandler<UnhandledMessageEventArgs> UnhandledMessageReceived;

        OperationResult ConnenctData();

        void DisconnenctData();

        OperationResult<T> SendMessage<T>(RequestMessage message) where T : BaseResponsMessage;

        OperationResult<RawResponseMessage> SendMessage(RequestMessage message);

        Task<OperationResult> ConnenctDataAsync();

        Task<OperationResult<T>> SendMessageAsync<T>(RequestMessage message) where T : BaseResponsMessage;

        Task<OperationResult<RawResponseMessage>> SendMessageAsync(RequestMessage message);


    }
}
