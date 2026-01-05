using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Course.GetCourseById;

public sealed record GetCourseByIdResponse(
    Guid Id,
    string Name,
    int CreditHours,
    string Description);

public sealed class GetCourseByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetCourseById/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCourseByIdQuery(id));

            return result.Match(
                onSuccess: () => Results.Ok(result.Value.ToGetCourseByIdResponse()),
                onFailure: error => Results.BadRequest(error));
        })
            .WithName("GetCourseById")
            .Produces<GetCourseByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Course by Id")
            .WithDescription("Get Course by Id");
    }
}
