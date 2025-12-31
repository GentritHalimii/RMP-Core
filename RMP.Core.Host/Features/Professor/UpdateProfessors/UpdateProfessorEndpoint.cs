using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Professor.UpdateProfessor;

public sealed record UpdateProfessorRequest(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Education,
    string Role);
public sealed record UpdateProfessorResponse(bool IsSuccess);

public sealed class UpdateProfessorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/UpdateProfessor", async (UpdateProfessorRequest request, ISender sender) =>
        {
            var command = request.ToUpdateProfessorCommand();

            var result = await sender.Send(command);
            return result.Match(
                onSuccess: () => Results.Ok(result.IsSuccess),
                onFailure: error => Results.BadRequest(error));
        })
            .WithName("UpdateProfessor")
            .Produces<UpdateProfessorResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Professor")
            .WithDescription("Update Professor");
    }
}