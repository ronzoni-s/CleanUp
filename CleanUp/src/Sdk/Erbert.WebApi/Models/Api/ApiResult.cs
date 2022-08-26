using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.WebApi.Sdk.Models
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResult(
            bool isSuccess
            , HttpStatusCode statusCode
            , string message = null
            )
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message ?? statusCode.ToString();
        }
    }

    public class ApiResult<TResponse> : ApiResult
        //where TResponse : class
    {
        public TResponse Response { get; set; }

        public ApiResult(
            bool isSuccess
            , HttpStatusCode statusCode
            , TResponse response
            , string message = null)
            : base(isSuccess, statusCode, message)
        {
            Response = response;
        }


        #region Implicit Operators
        public static implicit operator ApiResult<TResponse>(TResponse data)
        {
            return new ApiResult<TResponse>(true, HttpStatusCode.OK, data);
        }
        #endregion
    }
}
