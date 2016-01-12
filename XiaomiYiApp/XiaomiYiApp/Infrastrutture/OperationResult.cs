using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XiaomiYiApp.Model.Entities
{
    public class OperationResult
    {
        public Boolean Success { get; set; }
        public String ResultMessage { get; set; }

        public static OperationResult FromResult(OperationResult opResult)
        {
            return new OperationResult { ResultMessage = opResult.ResultMessage, Success = opResult.Success };
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Result { set; get; }

        public  OperationResult<T> SetSucces(T result)
        {

            Success = true;
            Result = result;
            return this;
        }

        public OperationResult<T> SetFail(String resultMessage)
        {

            Success = false;
            ResultMessage = resultMessage;
            return this;

        }

        public static OperationResult<T> GetSucces(T result)
        {
            return new OperationResult<T>
            {
                Success = true,
                Result = result,
            };
        }

        public static OperationResult<T> GetFail(String resultMessage)
        {
            return new OperationResult<T>
            {
                Success = false,
                ResultMessage = resultMessage,
            };
        }

        public static OperationResult<T> GetFail(String resultMessage, T result)
        {
            return new OperationResult<T>
            {
                Success = false,
                ResultMessage = resultMessage,
                Result = result,
            };
        }

        public static new OperationResult<T> FromResult(OperationResult opResult)
        {
            return new OperationResult<T> { ResultMessage = opResult.ResultMessage, Success = opResult.Success };
        }
    }
}
