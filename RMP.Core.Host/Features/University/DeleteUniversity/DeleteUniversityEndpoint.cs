using Carter;
using MediatR;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.University.DeleteUniversity;

public sealed record DeleteUniversityResponse(bool IsSuccess);

public sealed class DeleteUniversityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/DeleteUniversity/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteUniversityCommand(id));

                result.Match(
                    onSuccess: () => Results.Ok(result.IsSuccess),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("DeleteUniversity")
            .Produces<DeleteUniversityResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete University")
            .WithDescription("Delete University");
    }
}