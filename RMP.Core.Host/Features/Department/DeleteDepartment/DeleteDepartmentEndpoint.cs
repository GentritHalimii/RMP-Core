using Carter;
using MediatR;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.Department.DeleteDepartment;

public sealed record DeleteDepartmentResponse(bool IsSuccess);

public sealed class DeleteDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/DeleteDepartment/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteDepartmentCommand(id));

                result.Match(
                    onSuccess: () => Results.Ok(result.IsSuccess),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("DeleteDepartment")
            .Produces<DeleteDepartmentResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Department")
            .WithDescription("Delete Department");
    }
}