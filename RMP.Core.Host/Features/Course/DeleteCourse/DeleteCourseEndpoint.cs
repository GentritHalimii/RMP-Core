using Carter;
using MediatR;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.Course.DeleteCourse;

public sealed record DeleteCourseResponse(bool IsSuccess);

public sealed class DeleteCourseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/DeleteCourse/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteCourseCommand(id));

            result.Match(
                onSuccess: () => Results.Ok(result.IsSuccess),
                onFailure: error => Results.BadRequest(error));
        })
            .WithName("DeleteCourse")
            .Produces<DeleteCourseResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Course")
            .WithDescription("Delete Course");
    }
}