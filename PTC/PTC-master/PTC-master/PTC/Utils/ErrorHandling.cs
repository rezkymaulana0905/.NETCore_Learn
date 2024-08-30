using Microsoft.EntityFrameworkCore;
using System.Net;

namespace PTC.Utils;

public class ErrorHandling
{
    public static HttpStatusCode GetHttpStatusCode(Exception exception)
    {
        return exception switch
        {
            HttpRequestException _ => HttpStatusCode.BadGateway,
            TimeoutException _ => HttpStatusCode.RequestTimeout,
            DbUpdateException _ => HttpStatusCode.InternalServerError,
            Exception ex when ex.Message.Contains("Not Found") => HttpStatusCode.NotFound,
            Exception ex when ex.Message.StartsWith("Bad request") => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };
    }
}
