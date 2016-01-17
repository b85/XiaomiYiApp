using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XiaomiYiApp.Infrastrutture;
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Events;
using XiaomiYiApp.Model.Messages;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.Servicies
{
    class CameraConnectionService : ICameraConnectionService
    {

        private const int MAX_BUFFER_SIZE = 1024*4;
        private const int TIMEOUT_MILLISECONDS = 60000;
        private int _token;
        private Socket _sender;
        private List<RequestStateObjectBase> _requestList;

        public event EventHandler<UnhandledMessageEventArgs> UnhandledMessageReceived;

        public Boolean IsConnected
        {
            get { return _sender.Connected && _token > 0; }
        }

        public CameraConnectionService()
        {
            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _requestList = new List<RequestStateObjectBase>();
            _token = 0;
        }

        public OperationResult ConnenctData()
        {
            OperationResult result = ConnectSocket();
            if (!result.Success)
            {
                return result;
            }

            StartReceiveLoop();

            result = GetToken();
            if (result.Success)
            {
                _token = ((OperationResult<int>)result).Result;
            }

            return result;
        }

        public OperationResult<T> SendMessage<T>(RequestMessage message) where T : BaseResponseMessage
        {
            OperationResult<RawResponseMessage> rawResponse = SendMessage(message);
            if (rawResponse.Success)
            {
                JObject jObject = JObject.Parse(rawResponse.Result.JsonMessage);
                var responseMessage = jObject.ToObject<T>();
                if (responseMessage.Success)
                {
                    return OperationResult<T>.GetSucces(responseMessage);
                }
                else
                {
                    var opResult = OperationResult<T>.GetFail(responseMessage.Result.ToString());
                    opResult.Result = responseMessage;
                }
            }

            return OperationResult<T>.GetFail(rawResponse.ResultMessage);
        }

        public OperationResult<RawResponseMessage> SendMessage(RequestMessage message)
        {
            message.Token = _token;
            OperationResult<RawResponseMessage> result = new OperationResult<RawResponseMessage>();
            string json = JsonConvert.SerializeObject(message);
            byte[] msgBuffer = Encoding.UTF8.GetBytes(json);

            // Send the data through the socket.
            // BeginReceive2(message.MessageId);
            RequestStateObject stateObject = new RequestStateObject
            {
                CompletedWaitHandle = new ManualResetEvent(false),
                MessageId = message.MessageId,
            };
            _requestList.Add(stateObject);

            ManualResetEvent sendDone = new ManualResetEvent(false);
            SocketAsyncEventArgs socketEventArg = GetSocketEventArgs(msgBuffer);
            // Inline event handler for the Completed event.
            // Note: This event handler was implemented inline in order to make this method self-contained.
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                // Signal that the request is complete, unblocking the UI thread
                sendDone.Set();
            });



            _sender.SendAsync(socketEventArg);
            if (sendDone.WaitOne(TIMEOUT_MILLISECONDS))
            {
                if (socketEventArg.SocketError != SocketError.Success)
                {
                    result.Success = false;
                    result.ResultMessage = socketEventArg.SocketError.ToString();
                }
                else
                {
                    if (stateObject.CompletedWaitHandle.WaitOne(TIMEOUT_MILLISECONDS))
                    {
                        result.Success = true;
                        result.Result = stateObject.ResponseMessage;
                    }
                    else
                    {
                        result.Success = false;
                        result.ResultMessage = "Receive Timeout";
                    }
                }
            }
            else
            {
                result.Success = false;
                result.ResultMessage = "Send Timeout";
            }

            _requestList.Remove(stateObject);
            stateObject.CompletedWaitHandle.Dispose();
            sendDone.Dispose();

            Console.WriteLine("Response_Message: " + stateObject.ResponseMessage);

            return result;
        }

        public void DisconnenctData()
        {
            _sender.Shutdown(SocketShutdown.Both);
            _sender.Close();
            _token = 0;
        }

        public async Task<OperationResult<RawResponseMessage>> SendMessageAsync(RequestMessage message)
        {
            message.Token = _token;
            OperationResult<RawResponseMessage> result = new OperationResult<RawResponseMessage>();
            string json = JsonConvert.SerializeObject(message);
            byte[] msgBuffer = Encoding.UTF8.GetBytes(json);

            // Send the data through the socket.
            // BeginReceive2(message.MessageId);
            RequestStateObjectAsync stateObject = new RequestStateObjectAsync
            {
                ResponseTaskCompletionSource = GetTaskCompletionSource<RawResponseMessage>(),
                MessageId = message.MessageId,
            };
            _requestList.Add(stateObject);

           // ManualResetEvent sendDone = new ManualResetEvent(false);
            System.Threading.Tasks.TaskCompletionSource<Boolean> tcs = GetTaskCompletionSource<Boolean>();
            SocketAsyncEventArgs socketEventArg = GetSocketEventArgs(msgBuffer);
            // Inline event handler for the Completed event.
            // Note: This event handler was implemented inline in order to make this method self-contained.
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                // Signal that the request is complete, unblocking the UI thread
                tcs.SetResult(true);
            });



            _sender.SendAsync(socketEventArg);

            await tcs.Task;
            if (!tcs.Task.IsCanceled)
            {
                if (socketEventArg.SocketError != SocketError.Success)
                {
                    result.Success = false;
                    result.ResultMessage = socketEventArg.SocketError.ToString();
                }
                else
                {
                    await stateObject.ResponseTaskCompletionSource.Task;
                    if (!stateObject.ResponseTaskCompletionSource.Task.IsCanceled)
                    {
                        result.Success = true;
                        result.Result = stateObject.ResponseTaskCompletionSource.Task.Result;
                    }
                    else
                    {
                        result.Success = false;
                        result.ResultMessage = "Receive Timeout";
                    }
                }
            }
            else
            {
                result.Success = false;
                result.ResultMessage = "Send Timeout";
            }

            _requestList.Remove(stateObject);

        //    Console.WriteLine("Response_Message: " + stateObject.ResponseMessage);

            return result;
        }

        public async Task<OperationResult<T>> SendMessageAsync<T>(RequestMessage message) where T : BaseResponseMessage
        {
             OperationResult<RawResponseMessage> rawResponse = await SendMessageAsync(message);
            if (rawResponse.Success)
            {
                JObject jObject = JObject.Parse(rawResponse.Result.JsonMessage);
                var responseMessage = jObject.ToObject<T>();
                if (responseMessage.Success)
                {
                    return OperationResult<T>.GetSucces(responseMessage);
                }
                else
                {
                    return OperationResult<T>.GetFail(responseMessage.Result.ToString(), responseMessage);
                }
            }

            return OperationResult<T>.GetFail(rawResponse.ResultMessage);
        }

        public async Task<OperationResult> ConnenctDataAsync()
        {
            OperationResult result = await ConnectSocketAsync();
            if (!result.Success)
            {
                return result;
            }

            StartReceiveLoop();

            result = await GetTokenAsync();
            if (result.Success)
            {
                _token = ((OperationResult<int>)result).Result;
            }

            return result;
        }

        private TaskCompletionSource<T> GetTaskCompletionSource<T>(int timeout = TIMEOUT_MILLISECONDS)
        {
            System.Threading.Tasks.TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            CancellationTokenSource cts = new CancellationTokenSource(timeout);
            cts.Token.Register((param) =>
                                    {
                                        System.Threading.Tasks.TaskCompletionSource<T> tcsParam = (System.Threading.Tasks.TaskCompletionSource<T>)param;
                                        if (tcsParam.Task.Status != TaskStatus.RanToCompletion)
                                        {

                                            tcsParam.TrySetCanceled();
                                        }
                                    }, tcs);
            tcs.Task.ContinueWith(t =>
                                    { cts.Dispose(); });
            return tcs;
        }

        private OperationResult<int> GetToken()
        {
            OperationResult<int> opResult;
            RequestMessage request = new RequestMessage() { MessageId = 257, Token = 0 };
            OperationResult<ConnectResponseMessage> sendOpResult = SendMessage<ConnectResponseMessage>(request);
            if (sendOpResult.Success)
            {
                opResult = OperationResult<int>.GetSucces(sendOpResult.Result.Param);
                
            }
            else
            {
                opResult = OperationResult<int>.GetFail(sendOpResult.ResultMessage);
            }

            return opResult;
        }

        private async Task<OperationResult<int>> GetTokenAsync()
        {
            OperationResult<int> opResult;
            RequestMessage request = new RequestMessage() { MessageId = 257, Token = 0 };
            OperationResult<ConnectResponseMessage> sendOpResult = await SendMessageAsync<ConnectResponseMessage>(request);
            if (sendOpResult.Success)
            {
                opResult = OperationResult<int>.GetSucces(sendOpResult.Result.Param);

            }
            else
            {
                opResult = OperationResult<int>.GetFail(sendOpResult.ResultMessage);
            }

            return opResult;
        }

        private OperationResult ConnectSocket()
        {
            
            OperationResult result = new OperationResult() { Success = true, };
            ManualResetEvent connectDone = new ManualResetEvent(false);
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            socketEventArg.RemoteEndPoint = new System.Net.DnsEndPoint("192.168.42.1", 7878); 
            //DnsEndPoint

            // Inline event handler for the Completed event.
            // Note: This event handler was implemented inline in order to make this method self-contained.
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                // Retrieve the result of this request
                result.ResultMessage = e.SocketError.ToString();
                result.Success = e.SocketError == SocketError.Success;

                // Signal that the request is complete, unblocking the UI thread
                connectDone.Set();
            });

            //_sender.Connect("192.168.42.1", 7878);
            _sender.ConnectAsync(socketEventArg);

            if (connectDone.WaitOne(TIMEOUT_MILLISECONDS))
            {
                Console.WriteLine("Socket connected to {0}",
                    _sender.RemoteEndPoint.ToString());
            }
            else
            {
                result.Success = false;
                result.ResultMessage = "Socket Connection Timeout";
            }

            return result;
        }

        private async Task<OperationResult> ConnectSocketAsync()
        {

            OperationResult result = new OperationResult() { Success = true, };
            var tcs = GetTaskCompletionSource<Boolean>();
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            socketEventArg.RemoteEndPoint = new System.Net.DnsEndPoint("192.168.42.1", 7878);
            socketEventArg.UserToken = tcs;
            //DnsEndPoint

            // Inline event handler for the Completed event.
            // Note: This event handler was implemented inline in order to make this method self-contained.
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                // Retrieve the result of this request
              

                // Signal that the request is complete, unblocking the UI thread
                ((TaskCompletionSource<Boolean>)e.UserToken).SetResult(true);
            });

            //_sender.Connect("192.168.42.1", 7878);
            _sender.ConnectAsync(socketEventArg);

            
            try
            {
                await tcs.Task;
            }
            catch (AggregateException errors)
            {
                errors.Handle(e => e is TaskCanceledException);
            } 

            if (!tcs.Task.IsCanceled)
            {
                //Console.WriteLine("Socket connected to {0}",
                //    _sender.RemoteEndPoint.ToString());
                result.ResultMessage = socketEventArg.SocketError.ToString();
                result.Success = socketEventArg.SocketError == SocketError.Success || socketEventArg.SocketError == SocketError.IsConnected;
            }
            else
            {
                result.Success = false;
                result.ResultMessage = "Socket Connection Timeout";
            }

            return result;
        }

        private void StartReceiveLoop()
        {
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            socketEventArg.RemoteEndPoint = _sender.RemoteEndPoint;

            // Setup the buffer to receive the data
            socketEventArg.SetBuffer(new Byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);
            socketEventArg.Completed += (object s, SocketAsyncEventArgs e) =>
            {
                if (e.SocketError == SocketError.Success)
                {
                    //String json = Encoding.ASCII.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                    List<RawResponseMessage> respMessages = ParseReceivedMessage2(e.Buffer, e.Offset, e.BytesTransferred);
                    foreach (var rawMessage in respMessages)
                    {
                        RequestStateObjectBase stateObjectBase =
                            _requestList.FirstOrDefault(x => x.MessageId == rawMessage.MessageId);
                        if (stateObjectBase != null)
                        {
                            if (stateObjectBase is RequestStateObject)
                            {
                                RequestStateObject stateObject = (RequestStateObject)stateObjectBase;
                                stateObject.ResponseMessage = rawMessage;
                                stateObject.CompletedWaitHandle.Set();
                            }
                            else if (stateObjectBase is RequestStateObjectAsync)
                            {
                                RequestStateObjectAsync stateObject = (RequestStateObjectAsync)stateObjectBase;
                                stateObject.ResponseTaskCompletionSource.SetResult(rawMessage);
                            }
                        }
                        else
                        {
                            //TODO raise received msg event
                            //Console.WriteLine("Message_to_event: " + item.JsonMessage);
                            OnUnhadledMassageReceived(rawMessage);
                        }
                    }

                    Array.Clear(socketEventArg.Buffer, 0, socketEventArg.Buffer.Length);

                    _sender.ReceiveAsync(socketEventArg);
                }
                else
                {
                    // TODO handle errors
                }
            };
            _sender.ReceiveAsync(socketEventArg);
        }

        private List<RawResponseMessage> ParseReceivedMessage2(byte[] jsonMessage, int offset, int count)
        {
            List<RawResponseMessage> result = new List<RawResponseMessage>();
            using (MemoryStream mstream = new MemoryStream(jsonMessage, offset, count))
            {
                using (StreamReader sReader = new StreamReader(mstream))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(sReader))
                    {
                        JObject jObjectTest;
                        while (jsonReader.Read())
                        {
                            jObjectTest = JObject.Load(jsonReader);
                            int msgId = jObjectTest.Value<int>("msg_id");
                          //  Console.WriteLine("Message_2: " + jObjectTest.ToString(Formatting.None));
                          
                                result.Add( new RawResponseMessage
                                {
                                    JsonMessage = jObjectTest.ToString(Formatting.None),
                                    MessageId = msgId,
                                });                       
                        }
                    }
                }
            }
            return result;
        }

        private SocketAsyncEventArgs GetSocketEventArgs(byte[] msgBuffer)
        {
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            socketEventArg.RemoteEndPoint = _sender.RemoteEndPoint;
            socketEventArg.SetBuffer(msgBuffer, 0, msgBuffer.Length);
            return socketEventArg;
        }

        private void OnUnhadledMassageReceived(RawResponseMessage message)
        {
            if (UnhandledMessageReceived != null)
            {
                UnhandledMessageReceived(this, new UnhandledMessageEventArgs { Message = message });
            }
        }

        

    }
}
