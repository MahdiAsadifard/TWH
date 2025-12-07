using System.Net;

namespace Core.Response
{
    public class ServiceResponse<T> where T : class
    {
        public ServiceResponse(T data, HttpStatusCode httpStatus = HttpStatusCode.OK)
        {
            IsSuccess = true;
            StatusCode = httpStatus;
            Data = data;
        }
        public ServiceResponse(string message, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.OK)
            {
                throw new Exception("DO NOT use this construction for OK status.");
            }
            IsSuccess = false;
            Message = message;
            StatusCode = statusCode;
            Data = default(T);
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
    }
}
