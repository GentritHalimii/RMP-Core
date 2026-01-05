using Microsoft.EntityFrameworkCore;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;

namespace RMP.Host.Features.News.GetRecentNewsCount;

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