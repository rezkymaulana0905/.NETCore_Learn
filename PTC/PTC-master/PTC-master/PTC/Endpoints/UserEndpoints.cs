using Microsoft.Extensions.Options;
using PTC.Data;
using PTC.Services;
using PTC.Utils;
using System.Security.Claims;

namespace PTC.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/User").WithTags("User Auth");

            group.MapPost("/Login", async (PtcContext db, IOptions <JwtSettings> jwtSettings, string username, string password, string address) =>
            {
                try
                {
                    var login = await UserServices.Login(db, username, password, address, jwtSettings);
                    return login;
                }
                catch (Exception ex)
                {
                    var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                    return Results.Problem(
                        title: ex.Message,
                        statusCode: (int)statusCode
                    );
                }
            });

            group.MapPost("/ChangePassword", async (PtcContext db, HttpContext httpContext, IOptions<JwtSettings> jwtSettings, string password) =>
            {
                try
                {
                    var username = httpContext.User.FindFirstValue("UserName") ?? throw new Exception("username data Not Found");
                    var address = httpContext.User.FindFirstValue("Address") ?? throw new Exception("address data Not Found");
                    var login = await UserServices.ChangePassword(db,  username, password, address, jwtSettings);
                    return login;
                }
                catch (Exception ex)
                {
                    var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                    return Results.Problem(
                        title: ex.Message,
                        statusCode: (int)statusCode
                    );
                }
            }).RequireAuthorization();
        }
    }
}
