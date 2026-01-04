using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Rating.GetRateProfessorsById;

public sealed record GetRateProfessorsByIdQuery(Guid Id) : IQuery<Result<GetRateProfessorsByIdResult>>;

public sealed record GetRateProfessorsByIdResult(
    Guid Id,
    Guid ProfessorId,
    int UserId,
    int Overall,
    string Feedback,
    int? CommunicationSkills,
    int? Responsiveness,
    int? GradingFairness
);

internal sealed class GetRateProfessorsByIdQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetRateProfessorsByIdQuery, Result<GetRateProfessorsByIdResult>>
{
    public async Task<Result<GetRateProfessorsByIdResult>> Handle(GetRateProfessorsByIdQuery query, CancellationToken cancellationToken)
    {
        var professors = await dbContext
            .RateProfessors
            .FirstOrDefaultAsync(n => n.Id == query.Id, cancellationToken);

        if (professors is null)
            return Result.Failure<GetRateProfessorsByIdResult>(RateProfessorsErrors.NotFound(query.Id));

        return professors.ToGetRateProfessorsByIdResult();
    }
}