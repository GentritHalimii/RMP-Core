using Carter;
using MediatR;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.AdminDashboard.GetProfessorCount;

public sealed record GetProfessorCountResponse(int Count);

public sealed class GetProfessorCountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        
        app.MapGet("api/AdminDashboard/ProfessorCount", async (ISender sender) =>
            {
                var result = await sender.Send(new GetProfessorCountQuery());

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetProfessorCount")
            .Produces<GetProfessorCountResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Professor Count")
            .WithDescription("Retrieve the total number of professors.");
    }
}