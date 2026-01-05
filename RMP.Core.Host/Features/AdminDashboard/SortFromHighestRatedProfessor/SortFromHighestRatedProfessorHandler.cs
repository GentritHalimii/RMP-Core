
using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.AdminDashboard.GetHighestRatedProfessor;

public sealed record SortFromHighestRatedProfessorQuery() : IQuery<Result<GetHighestRatedProfessorResult>>;

public sealed record GetHighestRatedProfessorResult(
    Guid ProfessorId,
    double Overall,
    int? CommunicationSkills,
    int? Responsiveness,
    int? GradingFairness);

internal sealed class SortFromHighestRatedProfessorQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<SortFromHighestRatedProfessorQuery, Result<GetHighestRatedProfessorResult>>
{
    public async Task<Result<GetHighestRatedProfessorResult>> Handle(SortFromHighestRatedProfessorQuery query, CancellationToken cancellationToken)
    {
        var highestRatedProfessorEntity = await dbContext.RateProfessors
            .Include(rp => rp.Professor)
            .OrderByDescending(rp => rp.Overall)
            .FirstOrDefaultAsync(cancellationToken);

        var result = highestRatedProfessorEntity.ToGetHighestRatedProfessorResult();
        return Result.Success(result);
    }
}