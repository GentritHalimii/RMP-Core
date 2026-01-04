using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Rating.GetRateProfessors;

public sealed record GetRateProfessorsQuery() : IQuery<Result<IEnumerable<GetRateProfessorsResult>>>;

public sealed record GetRateProfessorsResult(
    Guid Id,
    Guid ProfessorId,
    int UserId,
    int Overall,
    string Feedback,
    int? CommunicationSkills,
    int? Responsiveness,
    int? GradingFairness);

internal sealed class GetRateProfessorsQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetRateProfessorsQuery, Result<IEnumerable<GetRateProfessorsResult>>>
{
    public async Task<Result<IEnumerable<GetRateProfessorsResult>>> Handle(GetRateProfessorsQuery query, CancellationToken cancellationToken)
    {
        var rateProfessors = await dbContext.RateProfessors
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var results = rateProfessors.Select(u => u.ToGetRateProfessorsResult());

        return Result.Success(results);
    }
}