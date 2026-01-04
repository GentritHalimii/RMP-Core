using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Rating.GetRateProfessorsByProfessorId;

public sealed record GetRateProfessorsByProfessorIdResponse(
    Guid Id,
    Guid ProfessorId,
    int UserId,
    int Overall,
    string Feedback,
    int? CommunicationSkills,
    int? Responsiveness,
    int? GradingFairness);

public sealed class GetRateProfessorsByProfessorIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetRateProfessorsByProfessorId/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetRateProfessorsByProfessorIdQuery(id));

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value.Select(r => r.ToGetRateProfessorsByProfessorIdResponse())),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetRateProfessorsByProfessorId")
            .Produces<IEnumerable<GetRateProfessorsByProfessorIdResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all RateProfessors by Professor Id")
            .WithDescription("Retrieve all RateProfessors ratings by the given Professor Id");
    }
}
