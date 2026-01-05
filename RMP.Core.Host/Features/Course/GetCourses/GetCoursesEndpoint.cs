using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Course.GetCourses;

public sealed record GetCoursesResponse(
    Guid Id,
    string Name,
    int CreditHours,
    string Description);

public sealed class GetCoursesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/Courses", async (ISender sender) =>
        {
            var result = await sender.Send(new GetCoursesQuery());

            return result.Match(
                onSuccess: () =>
                {
                    var departments = result.Value.Select(u => u.ToGetCoursesResponse());
                    return Results.Ok(departments);
                },
                onFailure: error => { return Results.BadRequest(error); });
        })
            .WithName("GetCourses")
            .Produces<IEnumerable<GetCoursesResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Courses")
            .WithDescription("Get Courses");
    }
}