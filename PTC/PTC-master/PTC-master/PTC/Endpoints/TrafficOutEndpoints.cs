using PTC.Data;
using PTC.Models;
using PTC.Services;
using PTC.Utils;
using System.Security.Claims;
namespace PTC.Endpoints;
public static class TrafficOutEndpoints
{
    public static void MapTrafficOutEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/TrafficOut").WithTags("TrafficOut");

        group.MapGet("/Out/{id}", async (string id, PtcContext db) =>
        {
            try
            {
                return await TrafficOutService.GetEmpLeave(id, db);
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
        .WithName("GetPopleOutById")
        .WithOpenApi();

        group.MapGet("/In/{id}", async (string id, PtcContext db) =>
        {
            try
            {
                return await TrafficOutService.GetEmpEnter(id, db);
            } catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                );
            }
        })
       .WithName("GetPopleReturnById")
       .WithOpenApi();

        group.MapPatch("/Out/{seqNo}", async (PtcContext db, HttpContext httpContext, int seqNo) =>
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
                return await TrafficOutService.ConfirmLeave(db, userAuth, seqNo);
            } catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                );
            }

        })
       .WithName("ConfirmPeopleOutById")
       .WithOpenApi()
       .RequireAuthorization();

        group.MapPatch("/In/{id}", async  (int id, HttpContext httpContext, PtcContext db) =>
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
                return await TrafficOutService.ConfirmEnter(id, userAuth, db);
            } catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                );
            }
        })
        .WithName("ConfirmPopleReturnById")
        .WithOpenApi()
        .RequireAuthorization();
    }
}