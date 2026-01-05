using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Course.UpdateCourse;

public sealed record UpdateCourseRequest(
    Guid Id,
    string Name,
    int CreditHours,
    string Description);
public sealed record UpdateCourseResponse(bool IsSuccess);

public sealed class UpdateCourseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/UpdateCourse", async (UpdateCourseRequest request, ISender sender) =>
        {
            var command = request.ToUpdateCourseCommand();

            var result = await sender.Send(command);
            return result.Match(
                onSuccess: () => Results.Ok(result.IsSuccess),
                onFailure: error => Results.BadRequest(error));
        })
            .WithName("UpdateCourse")
            .Produces<UpdateCourseResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Course")
            .WithDescription("Update Course");
    }
}