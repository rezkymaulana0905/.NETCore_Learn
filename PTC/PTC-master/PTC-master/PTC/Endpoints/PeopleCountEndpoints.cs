using PTC.Data;
using PTC.Services;
using PTC.Utils;

namespace PTC.Endpoints
{
    public static class PeopleCountEndpoints
    {
        public static void MapPeopleCountEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/People").WithTags("PeopleCount");

            group.MapGet("/", async (PtcContext db) =>
            {
                try
                {
                    var count = await PeopleCountService.GetPeopleCount(db);
                    return Results.Ok(count);
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

            group.MapGet("/{Department}", async  (PtcContext db, string department) =>
            {
                try
                {
                    var count = await PeopleCountService.DepartmentPeopleCount(db, department);
                    return Results.Ok(count);
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
        }

        
    }
}
