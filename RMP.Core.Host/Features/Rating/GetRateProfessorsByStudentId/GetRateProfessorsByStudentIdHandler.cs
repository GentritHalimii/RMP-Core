using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Rating.GetRateProfessorsByStudentId;

public sealed record GetRateProfessorsByStudentIdQuery(int Id) : IQuery<Result<IEnumerable<GetRateProfessorsByStudentIdResult>>>;

public sealed record GetRateProfessorsByStudentIdResult(
    Guid Id,
    Guid ProfessorId,
    int UserId,
    int Overall,
    string Feedback,
    int? CommunicationSkills,
    int? Responsiveness,
    int? GradingFairness
);

internal sealed class GetRateProfessorsByStudentIdQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetRateProfessorsByStudentIdQuery, Result<IEnumerable<GetRateProfessorsByStudentIdResult>>>
{
    public async Task<Result<IEnumerable<GetRateProfessorsByStudentIdResult>>> Handle(GetRateProfessorsByStudentIdQuery query, CancellationToken cancellationToken)
    {
        var ratings = await dbContext
            .RateProfessors
            .Where(r => r.UserId == query.Id)
            .ToListAsync(cancellationToken);

        if (!ratings.Any())
            return Result.Failure<IEnumerable<GetRateProfessorsByStudentIdResult>>(RateProfessorsErrors.NotFound(query.Id));
        
        var results = ratings.Select(u => u.ToGetRateProfessorsByStudentIdResult());

        return Result.Success(results);
    }
}