using System;
using System.Data;
using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Seats4Me.API.Common
{
    public class Seats4MeExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var status = HttpStatusCode.InternalServerError;
            var message = String.Empty;

            var exceptionType = context.Exception.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Unauthorized Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "A server error occurred.";
                status = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(DataException))
            {
                message = context.Exception.Message;
                status = HttpStatusCode.BadRequest;
            }
            else
            {
                message = context.Exception.Message;
                status = HttpStatusCode.NotFound;
            }

            context.ExceptionHandled = true;

            var response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            var err = message + " " + context.Exception.StackTrace;
            response.WriteAsync(err);
        }
    }
}