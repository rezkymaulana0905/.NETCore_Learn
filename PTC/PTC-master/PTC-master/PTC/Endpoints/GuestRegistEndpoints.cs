using Microsoft.EntityFrameworkCore;
using PTC.Data; 
using PTC.Models;
using PTC.Services;
using PTC.Utils;
using System.Security.Claims;
namespace PTC.Endpoints;

public static class GuestRegistEndpoints
{
    public static void MapGuestRegistEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/GuestRegist").WithTags(nameof(GuestReg));

        group.MapGet("/", async (PtcContext db) => await db.RegGuest.ToListAsync())
        .WithName("GetAllGuestRegists")
        .WithOpenApi();

        group.MapGet("/{id}", async (string id, PtcContext db) =>
        {
            try
            {
                var result = await GuestService.GetGuestById(id, db);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                );
            }
        })
        .WithName("GetGuestRegistById")
        .WithOpenApi();

        group.MapPatch("/In/{id}/{total}", async (string id, int total, PtcContext db, HttpContext httpContext) =>
        {
            try
            {
                var userAuth = new UserAuth
                {
                    UserName = httpContext.User.FindFirstValue("UserName")
                    ?? throw new Exception("Token Properties 'UserName' Not Found"),

                    UserId = int.Parse(httpContext.User.FindFirstValue("UserId")
                    ?? throw new Exception("Token Properties 'UserId' Not Found")),

                    LoginId = int.Parse(httpContext.User.FindFirstValue("LoginId")
                    ?? throw new Exception("Token Properties 'LoginId' Not Found"))
                };

                return await GuestService.ConfirmEnter(id, total, userAuth, db);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Wrong Date")
                {
                    return Results.Problem(
                            title: ex.Message,
                            statusCode: StatusCodes.Status403Forbidden
                        );
                } else
                {
                    var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                    return Results.Problem(
                        title: ex.Message,
                        statusCode: (int)statusCode
                    );
                }
            }
        })
        .WithName("SetConfirmInStatus")
        .WithOpenApi()
        .RequireAuthorization();

        group.MapPatch("/Out/{id}", async (string  id, PtcContext db, HttpContext httpContext) =>
        {
            try
            {
                var userAuth = new UserAuth
                {
                    UserName = httpContext.User.FindFirstValue("UserName")
                    ?? throw new Exception("Token Properties 'UserName' Not Found"),

                    UserId = int.Parse(httpContext.User.FindFirstValue("UserId") 
                    ?? throw new Exception("Token Properties 'UserId' Not Found")),

                    LoginId = int.Parse(httpContext.User.FindFirstValue("LoginId")
                    ?? throw new Exception("Token Properties 'LoginId' Not Found"))
                };

                return await GuestService.ConfirmLeave(id, userAuth, db);
            } catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                );
            }
        })
        .WithName("SetConfirmOutStatus")
        .WithOpenApi()
        .RequireAuthorization();
    }
}