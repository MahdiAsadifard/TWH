using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class ApiException : Exception
    {
        public ApiExceptionCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        
        /// <summary>
        /// Initialize a new instance of the <see cref="ApiException"/>
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        /// <param name="statusCode"></param>
        /// <param name="innerException"></param>
        public ApiException(
            ApiExceptionCode errorCode,
            string errorMessage,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
            Exception innerException = null
            ):base($": {errorMessage}, StatusCode: {statusCode}", innerException)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.StatusCode = statusCode;
        }
    }
}
