using System.Net;

namespace ApiTask.Exceptions
{
    public class HttpException(string message, HttpStatusCode httpStatusCode) : Exception(message)
    {
        public HttpStatusCode HttpStatusCode { get; set; } = httpStatusCode;
    }
}
