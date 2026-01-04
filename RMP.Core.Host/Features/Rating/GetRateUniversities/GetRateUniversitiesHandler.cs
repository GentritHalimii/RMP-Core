using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Rating.GetRateUniversities;

public sealed record GetRateUniversitiesQuery() : IQuery<Result<IEnumerable<GetRateUniversitiesResult>>>;

public sealed record GetRateUniversitiesResult(
    Guid Id,
    Guid UniversityId,
    int UserId,
    int Overall,
    string Feedback);

internal sealed class GetRateUniversitiesQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetRateUniversitiesQuery, Result<IEnumerable<GetRateUniversitiesResult>>>
{
    public async Task<Result<IEnumerable<GetRateUniversitiesResult>>> Handle(GetRateUniversitiesQuery query, CancellationToken cancellationToken)
    {
        var rateUniversities = await dbContext.RateUniversities
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var results = rateUniversities.Select(u => u.ToGetRateUniversitiesResult());

        return Result.Success(results);
    }
}