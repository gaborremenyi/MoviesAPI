using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using MoviesAPI.Exceptions;
using System.Net;
using System.Text;

namespace MoviesAPI.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {

        public async override void OnException(ExceptionContext context)
        {
            HttpStatusCode status;
            string message;

            if (context.Exception is HttpException)
                status = ((HttpException)context.Exception).StatusCode;
            else
                status = HttpStatusCode.InternalServerError;

            message = context.Exception.Message;

            HttpResponse response = context.HttpContext.Response;
            var error = "{ \"Code\": \"" + (int)status + "\", \"Status\": \"" + status + "\", \"Message\":\"" + message + "\" }";
            byte[] data = Encoding.UTF8.GetBytes(error);
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            response.ContentLength = data.Length;
            await response.WriteAsync(error, Encoding.UTF8);
        }
    }
}
