using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.AdminDashboard.GetHighestRatedUniversity;

public sealed record GetHighestRatedUniversityQuery() : IQuery<Result<GetHighestRatedUniversityResult>>;

public sealed record GetHighestRatedUniversityResult(
    Guid UniversityId);

internal sealed class GetHighestRatedUniversityQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetHighestRatedUniversityQuery, Result<GetHighestRatedUniversityResult>>
{
    public async Task<Result<GetHighestRatedUniversityResult>> Handle(GetHighestRatedUniversityQuery query, CancellationToken cancellationToken)
    {
        var highestRatedUniversityEntity = await dbContext.RateUniversities
            .Include(ru => ru.University)
            .AsNoTracking()
            .OrderByDescending(ru => ru.Overall)
            .FirstOrDefaultAsync(cancellationToken);

        var result = highestRatedUniversityEntity.ToGetHighestRatedUniversityResult();
        return Result.Success(result);
    }
}