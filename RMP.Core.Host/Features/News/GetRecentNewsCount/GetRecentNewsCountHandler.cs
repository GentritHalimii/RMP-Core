using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;

namespace RMP.Core.Host.Features.News.GetRecentNewsCount;

public sealed record GetRecentNewsCountQuery() : IQuery<Result<IEnumerable<DayNewsCountResult>>>;

public sealed record DayNewsCountResult(
    DateTime Date,
    int Count
);

internal sealed class GetRecentNewsCountQueryHandler(ApplicationDbContext dbContext) 
    : IQueryHandler<GetRecentNewsCountQuery, Result<IEnumerable<DayNewsCountResult>>>
{
    public async Task<Result<IEnumerable<DayNewsCountResult>>> Handle(
        GetRecentNewsCountQuery query, 
        CancellationToken cancellationToken)
    {
        var recentNewsCount = await dbContext.News
            .GroupBy(n => n.PublicationDate.Date)
            .Select(g => new DayNewsCountResult(g.Key, g.Count())) 
            .OrderByDescending(d => d.Date)
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<DayNewsCountResult>>(recentNewsCount);
    }
}