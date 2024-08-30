using PTC.Models;
using PTC.Data;
using PTC.Services;
using PTC.Utils;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PTC.Endpoints;

public static class SpplierEndpoints
{
    public static void MapVhcSpplierEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Spplier").WithTags(nameof(VhcSpplier));

        group.MapGet("/", async (PtcContext db) =>
        {
            try
            {
                return await SpplierService.GetSpplier(db);
            } catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                );
            }
        })
        .WithName("GetAllSppliers")
        .WithOpenApi();

        group.MapPost("/", async (PtcContext db, HttpContext httpContext, Spplier add) =>
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

                return await SpplierService.CreateSpplierData(add, userAuth, db);
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
        .WithName("CreateSpplier")
        .RequireAuthorization()
        .WithOpenApi();

        group.MapPatch("/", async (PtcContext db, HttpContext httpContext, ConfirmVhc conf) =>
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

                return await SpplierService.ConfirmSupplierOut(conf, userAuth, db);
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
        .WithName("ConfirmOut")
        .RequireAuthorization()
        .WithOpenApi();
    }
}