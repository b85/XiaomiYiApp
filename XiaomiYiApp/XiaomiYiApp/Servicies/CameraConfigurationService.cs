﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaomiYiApp.Infrastrutture;
using XiaomiYiApp.Model.Entities;
using XiaomiYiApp.Model.Enums;
using XiaomiYiApp.Model.Messages;
using XiaomiYiApp.Servicies.Interfaces;

namespace XiaomiYiApp.Servicies
{
    public class CameraConfigurationService : ICameraConfigurationService 
    {
        private ICameraConnectionService _connectionManager;

        public CameraConfigurationService(ICameraConnectionService cameraConnectionService)
        {
            _connectionManager = cameraConnectionService;
        }


        #region Sync
        public OperationResult<Configuration> GetConfiguration()
        {
            var opResult = _connectionManager.SendMessage<FullConfigResponseMessage>(new RequestMessage { MessageId = (int)MessageType.ConfigurationGet });
            if (opResult.Success)
            {
                return OperationResult<Configuration>.GetSucces(new Configuration { Parameters = Helpers.GetConfigurationFromMessage(opResult.Result) });
            }

            return OperationResult<Configuration>.GetFail(opResult.ResultMessage);
        }


        public OperationResult<DetailedConfiguration> GetDetailedConfiguration()
        {
            OperationResult<DetailedConfiguration> opResult = new OperationResult<DetailedConfiguration>();
            var configOpResult = GetConfiguration();
            if (configOpResult.Success)
            {
                List<ConfigurationParameterDetail> details = new List<ConfigurationParameterDetail>();
                foreach (var param in configOpResult.Result.Parameters)
                {

                    var detailOpResult = _connectionManager.SendMessage<ConfigDetailResponseMessage>(new RequestMessageWithParam
                    {
                        MessageId = (int)MessageType.ConfigurationGet,
                        Param = param.Name,
                    });

                    if (detailOpResult.Success)
                    {
                        details.Add(Helpers.GetParameterDetailFromMessage(detailOpResult.Result));
                        details.Last().Value = param.Value;
                    }
                    else
                    {
                        return opResult.SetFail(detailOpResult.ResultMessage);
                        //return OperationResult<List<ConfigParameterDetail>>.GetFail(detailOpResult.ResultMessage);
                    }
                }

                //return OperationResult<List<ConfigParameterDetail>>.GetSucces(details);
                return opResult.SetSucces(new DetailedConfiguration { Parameters = details });
            }

            return opResult.SetFail(configOpResult.ResultMessage);
            //return OperationResult<List<ConfigParameterDetail>>.GetFail(configOpResult.ResultMessage);
        }

        public OperationResult SetConfigurationParameter(String name, String value)
        {
            RequestMessageWithParamType requestMsg = new RequestMessageWithParamType
            {
                MessageId = (int)MessageType.ConfigurationSet,
                Param = value,
                Type = name,
            };

            var opRes = _connectionManager.SendMessage<BaseResponsMessage>(requestMsg);

            return new OperationResult { Success = opRes.Success, ResultMessage = opRes.ResultMessage };
            //'{"msg_id":2,"token":%s, "type":"%s", "param":"%s"}'
        }
        #endregion

        #region Async
        public async Task<OperationResult<Configuration>> GetConfigurationAsync()
        {
            var opResult = await _connectionManager.SendMessageAsync<FullConfigResponseMessage>(new RequestMessage { MessageId = (int)MessageType.ConfigurationGet });
            if (opResult.Success)
            {
                return OperationResult<Configuration>.GetSucces(new Configuration { Parameters = Helpers.GetConfigurationFromMessage(opResult.Result) });
            }

            return OperationResult<Configuration>.GetFail(opResult.ResultMessage);
        }


        public async Task<OperationResult<DetailedConfiguration>> GetDetailedConfigurationAsync()
        {
            OperationResult<DetailedConfiguration> opResult = new OperationResult<DetailedConfiguration>();
            var configOpResult = await GetConfigurationAsync();
            if (configOpResult.Success)
            {
                List<ConfigurationParameterDetail> details = new List<ConfigurationParameterDetail>();
                foreach (var param in configOpResult.Result.Parameters)
                {

                    var detailOpResult = await _connectionManager.SendMessageAsync<ConfigDetailResponseMessage>(new RequestMessageWithParam
                    {
                        MessageId = (int)MessageType.ConfigurationGet,
                        Param = param.Name,
                    });

                    if (detailOpResult.Success)
                    {
                        details.Add(Helpers.GetParameterDetailFromMessage(detailOpResult.Result));
                        details.Last().Value = param.Value;
                    }
                    else
                    {
                        return opResult.SetFail(detailOpResult.ResultMessage);
                        //return OperationResult<List<ConfigParameterDetail>>.GetFail(detailOpResult.ResultMessage);
                    }
                }

                //return OperationResult<List<ConfigParameterDetail>>.GetSucces(details);
                return opResult.SetSucces(new DetailedConfiguration { Parameters = details });
            }

            return opResult.SetFail(configOpResult.ResultMessage);
            //return OperationResult<List<ConfigParameterDetail>>.GetFail(configOpResult.ResultMessage);
        }

        public async Task<OperationResult> SetConfigurationParameterAsync(String name, String value)
        {
            RequestMessageWithParamType requestMsg = new RequestMessageWithParamType
            {
                MessageId = (int)MessageType.ConfigurationSet,
                Param = value,
                Type = name,
            };

            var opRes = await _connectionManager.SendMessageAsync<BaseResponsMessage>(requestMsg);

            return new OperationResult { Success = opRes.Success, ResultMessage = opRes.ResultMessage };
            //'{"msg_id":2,"token":%s, "type":"%s", "param":"%s"}'
        }
        #endregion
    }
}