using Carter;
using MediatR;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.Professor.DeleteProfessor;

public sealed record DeleteProfessorResponse(bool IsSuccess);

public sealed class DeleteProfessorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/DeleteProfessor/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProfessorCommand(id));

            result.Match(
                onSuccess: () => Results.Ok(result.IsSuccess),
                onFailure: error => Results.BadRequest(error));
        })
            .WithName("DeleteProfessor")
            .Produces<DeleteProfessorResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Professor")
            .WithDescription("Delete Professor");
    }
}