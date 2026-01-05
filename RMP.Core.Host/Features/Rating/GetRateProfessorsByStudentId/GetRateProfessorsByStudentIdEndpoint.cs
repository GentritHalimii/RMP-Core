using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Rating.GetRateProfessorsByStudentId;

public sealed record GetRateProfessorsByStudentIdResponse(
    Guid Id,
    Guid ProfessorId,
    int UserId,
    int Overall,
    string Feedback,
    int? CommunicationSkills,
    int? Responsiveness,
    int? GradingFairness);

public sealed class GetRateProfessorsByStudentIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetRateProfessorsByStudentId/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetRateProfessorsByStudentIdQuery(id));

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value.Select(r => r.ToGetRateProfessorsByStudentIdResponse())),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetRateProfessorsByStudentId")
            .Produces<IEnumerable<GetRateProfessorsByStudentIdResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all RateProfessors by student Id")
            .WithDescription("Retrieve all RateProfessors ratings by the given student Id");
    }
}