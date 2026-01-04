using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Rating.GetRateProfessors;

public sealed record GetRateProfessorsResponse(
    Guid Id,
    Guid ProfessorId,
    int UserId,
    int Overall,
    string Feedback,
    int? CommunicationSkills,
    int? Responsiveness,
    int? GradingFairness);

public sealed class GetRateProfessorsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/RateProfessors", async (ISender sender) =>
            {
                var result = await sender.Send(new GetRateProfessorsQuery());

                return result.Match(
                    onSuccess: () =>
                    {
                        var rateProfessors = result.Value.Select(u => u.ToGetRateProfessorsResponse());
                        return Results.Ok(rateProfessors);
                    },
                    onFailure: error => { return Results.BadRequest(error); });
            })
            .WithName("GetRateProfessors")
            .Produces<IEnumerable<GetRateProfessorsResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get RateProfessors")
            .WithDescription("Get RateProfessors");
    }
}