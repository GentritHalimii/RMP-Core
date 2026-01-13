using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.News.GetThreeLatestCreatedNews;


public sealed record GetThreeLatestCreatedNewsQuery() : IQuery<Result<IEnumerable<GetThreeLatestCreatedNewsResult>>>;

public sealed record GetThreeLatestCreatedNewsResult(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath);

internal sealed class GetThreeLatestCreatedNewsHandler(ApplicationDbContext dbContext) : IQueryHandler<GetThreeLatestCreatedNewsQuery, Result<IEnumerable<GetThreeLatestCreatedNewsResult>>>
{
    public async Task<Result<IEnumerable<GetThreeLatestCreatedNewsResult>>> Handle(GetThreeLatestCreatedNewsQuery query, CancellationToken cancellationToken)
    {
        var latestNews = await dbContext.News
            .OrderByDescending(n => n.PublicationDate) 
            .Take(3)
            .ToListAsync(cancellationToken);
        
        var result = latestNews.Select(news => news.ToGetThreeLatestCreatedNewsResult());

        return Result.Success(result);
    }
}