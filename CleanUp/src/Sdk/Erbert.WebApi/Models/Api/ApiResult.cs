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

        public IDictionary<string, string[]> Validations { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }

        public ApiResult(
            bool isSuccess
            , HttpStatusCode statusCode
            , string message = null
            , IDictionary<string, string[]> validations = null
            , Dictionary<string, object> additionalData = null)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message ?? statusCode.ToString();
            Validations = validations;
            AdditionalData = additionalData;
        }

    }

    public class ApiResult<TResponse> : ApiResult
        where TResponse : class
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

    public class ApiPaginatedResult<TResponse> : ApiResult
       where TResponse : class
    {

        public List<TResponse> Response { get; set; }

        public PaginationResult Pagination { get; set; }

        public ApiPaginatedResult(
           bool isSuccess
           , HttpStatusCode statusCode
           , PaginationResult pagination
           , List<TResponse> response
           , string message = null)
            : base(isSuccess, statusCode, message)
        {
            Pagination = pagination;
            Response = response;
        }

    }
}
