using System.Net;

namespace ApiTask.Exceptions
{
    public class ForbiddenAccessException(string message) : HttpException(message, HttpStatusCode.Forbidden)
    {
    }
}
