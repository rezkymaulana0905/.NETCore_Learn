using Newtonsoft.Json;
using PTC.Services;
using PTC.Data;
using PTC.Utils;
using PTC.Models;
namespace PTC.Endpoints;


public static class ActivityLogEndpoints
{
    public static void MapActivityLogEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Recent").WithTags("Log Activity");

        group.MapGet("/", async (PtcContext db) =>
        {
            try
            {
                var Plan = await LogActivityService.GetPlan(db);
                var In = await LogActivityService.GetIn(db);
                var Out = await LogActivityService.GetOut(db);

                var data = new
                {
                    Plan,
                    In,
                    Out,
                };

                return Results.Ok(data);
            } catch (Exception ex)
            {
                var statusCode = ErrorHandling.GetHttpStatusCode(ex);
                return Results.Problem(
                    title: ex.Message,
                    statusCode: (int)statusCode
                );
            }
        });
    }
}