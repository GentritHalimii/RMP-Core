using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Rating.GetRateProfessorsByProfessorId;

public sealed record GetRateProfessorsByProfessorIdQuery(Guid Id) : IQuery<Result<IEnumerable<GetRateProfessorsByProfessorIdResult>>>;

public sealed record GetRateProfessorsByProfessorIdResult(
    Guid Id,
    Guid ProfessorId,
    int UserId,
    int Overall,
    string Feedback,
    int? CommunicationSkills,
    int? Responsiveness,
    int? GradingFairness
);

internal sealed class GetRateProfessorsByProfessorIdQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetRateProfessorsByProfessorIdQuery, Result<IEnumerable<GetRateProfessorsByProfessorIdResult>>>
{
    public async Task<Result<IEnumerable<GetRateProfessorsByProfessorIdResult>>> Handle(GetRateProfessorsByProfessorIdQuery query, CancellationToken cancellationToken)
    {
        var ratings = await dbContext
            .RateProfessors
            .Where(r => r.ProfessorId == query.Id)
            .ToListAsync(cancellationToken);

        if (!ratings.Any())
            return Result.Failure<IEnumerable<GetRateProfessorsByProfessorIdResult>>(RateProfessorsErrors.NotFound(query.Id));
        
        var results = ratings.Select(u => u.ToGetRateProfessorsByProfessorIdResult());

        return Result.Success(results);
    }
}
