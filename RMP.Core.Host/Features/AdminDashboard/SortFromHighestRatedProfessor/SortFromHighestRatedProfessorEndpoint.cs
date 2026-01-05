
using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.AdminDashboard.GetHighestRatedProfessor;

public sealed record GetHighestRatedProfessorResponse(
    Guid ProfessorId,
    double Overall,
    int? CommunicationSkills,
    int? Responsiveness,
    int? GradingFairness);

public sealed class SortFromHighestRatedProfessorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/AdminDashboard/HighestRatedProfessor", async (ISender sender) =>
            {
                var result = await sender.Send(new SortFromHighestRatedProfessorQuery());

                return result.Match(
                    onSuccess: () =>
                    {
                        var response = result.Value.ToGetHighestRatedProfessorResponse();
                        return Results.Ok(response);
                    },
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("SortFromHighestRatedProfessor")
            .Produces<GetHighestRatedProfessorResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Highest Rated Professor")
            .WithDescription("Retrieve the highest-rated professor based on their overall rating.");
    }
}