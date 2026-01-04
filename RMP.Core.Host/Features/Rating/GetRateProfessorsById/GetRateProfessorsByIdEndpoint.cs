using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Rating.GetRateProfessorsById;

public sealed record GetRateProfessorsByIdResponse(
    Guid Id,
    Guid ProfessorId,
    int UserId,
    int Overall,
    string Feedback,
    int? CommunicationSkills,
    int? Responsiveness,
    int? GradingFairness);

public sealed class GetRateProfessorsByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetRateProfessorsById/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetRateProfessorsByIdQuery(id));

            return result.Match(
                onSuccess: () => Results.Ok(result.Value.ToGetRateProfessorsByIdResponse()),
                onFailure: error => Results.BadRequest(error));
        })
            .WithName("GetRateProfessorsById")
            .Produces<GetRateProfessorsByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get RateProfessors by Id")
            .WithDescription("Get RateProfessors by Id");
    }
}