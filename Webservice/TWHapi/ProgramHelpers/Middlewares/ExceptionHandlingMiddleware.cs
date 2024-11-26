namespace TWHapi.ProgramHelpers.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Core.Exceptions;
    using System.Net;
    using Microsoft.AspNetCore.Diagnostics;
    using Newtonsoft.Json;
    using Core.NLogs;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _nextDelegate;
        public ExceptionHandlingMiddleware(RequestDelegate nextDelegate)
        {
            _nextDelegate = nextDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ArgumentsValidator.ThrowIfNull(nameof(context), context);

            try
            {
                await _nextDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static Task HandleException(HttpContext context, Exception exception)
        {
            NLogHelpers<ExceptionHandlerMiddleware>.Logger.Error(
                $"\n========START==========\n Error on ExceptionHandlerMiddleware: \n {JsonConvert.SerializeObject(exception)} \n =========END=========");

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = string.Empty;


            context.Response.ContentType = "application/json";

            if (exception is ApiException apiException)
            {
                context.Response.StatusCode = (int)apiException.StatusCode;
                return context.Response.WriteAsJsonAsync(
                    new
                    {
                        apiException.Message,
                        apiException.StatusCode,
                        //Detail = apiException.StackTrace
                    }
                    );
            }

            switch (exception)
            {
                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "A required argument was null";
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = "The requested resource was not found";
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "Unauthorized access";
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An unexpected error eccured";
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsJsonAsync(
                new
                {
                    StatusCode = statusCode,
                    Message = message,
                    //Detail = exception,
                });
        }
    }
}
