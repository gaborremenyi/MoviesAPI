using System;
using System.Net;

namespace MoviesAPI.Exceptions
{
    public class HttpException : Exception
    {
        private readonly HttpStatusCode httpStatusCode;

        public HttpException(int httpStatusCode) : base()
        {
            this.httpStatusCode = (HttpStatusCode)httpStatusCode;
        }

        public HttpException(HttpStatusCode httpStatusCode) : base()
        {
            this.httpStatusCode = httpStatusCode;
        }

        public HttpException(int httpStatusCode, string message) : base(message)
        {
            this.httpStatusCode = (HttpStatusCode)httpStatusCode;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            this.httpStatusCode = httpStatusCode;
        }

        public HttpException(int httpStatusCode, string message, Exception inner) : base(message, inner)
        {
            this.httpStatusCode = (HttpStatusCode)httpStatusCode;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message, Exception inner) : base(message, inner)
        {
            this.httpStatusCode = httpStatusCode;
        }

        public HttpStatusCode StatusCode { get { return this.httpStatusCode; } }
    }
}
