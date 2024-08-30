using PTC.Services;
using PTC.Models; 
using PTC.Data;
using PTC.Utils;
using System.Security.Claims;
namespace PTC.Endpoints;

public static class WorkPermitEndpoints
{
    public static void MapWorkPermitEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/WorkPermit").WithTags("WorkPermit");

        group.MapGet("/{id}", async (int id, PtcContext db) =>
        {
            try
            {
                return await WorkPermitService.GetWorker(id, db);
            } catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                );
            }
        })
        .WithName("getWorker")
        .WithOpenApi();

        group.MapGet("/Record/{start}/{end}", async (DateOnly start, DateOnly end, PtcContext db) =>
        {
            try
            {
                return await WorkPermitService.GetWorkerRange(start, end, db);
            } catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                );
            }
        })
        .WithName("getRecord")
        .WithOpenApi();

        group.MapGet("/Actual", async (PtcContext db) =>
        {
            try
            {
                return await WorkPermitService.GetActual(db);
            } catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                );
            }
        })
        .WithName("GetActual")
        .WithOpenApi();

        group.MapGet("/ActiveWorker/{reqNum}/{date}", async (string reqNum, DateOnly date, PtcContext db) =>
        {
            try
            {
                return await WorkPermitService.GetActiveWorker(reqNum, date, db);
            } catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                );
            }
        })
        .WithName("GetActiveWorker")
        .WithOpenApi();

        group.MapPatch("/In/{id}", async (PtcContext db, HttpContext httpContext, int id) =>
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
                await WorkPermitService.ConfirmIn(db, userAuth, id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message == "The Worker Found haven't scanned out")
                {
                    return Results.Problem(
                        title: ex.Message,
                        statusCode: StatusCodes.Status403Forbidden
                    );
                }
                else
                {
                    var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                    return Results.Problem(
                        title: ex.Message,
                        statusCode: (int)statusCode
                    );
                }
            }

        })
        .WithName("WorkerIn")
        .RequireAuthorization()
        .WithOpenApi();

        group.MapPatch("/Out/{id}", async (PtcContext db, HttpContext httpContext, int id) =>
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
                await WorkPermitService.ConfirmLeave(db, userAuth, id);
                return Results.Ok();
            } catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                    );
            }
        })
        .WithName("WorkerOut")
        .RequireAuthorization()
        .WithOpenApi();

        group.MapPost("/", async (WorkPermit add, PtcContext db) =>
        {
            try
            {
                await WorkPermitService.AddWorkerData(add, db);
            } catch ( Exception ex )
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                    );
            }
            return TypedResults.Ok(add);
        })
        .WithName("PostWorkPermit")
        .WithOpenApi();
    }
}