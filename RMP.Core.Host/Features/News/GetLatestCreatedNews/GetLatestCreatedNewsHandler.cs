using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.News.GetLatestCreatedNews;

public sealed record GetLatestCreatedNewsQuery() : IQuery<Result<GetLatestCreatedNewsResult>>;

public sealed record GetLatestCreatedNewsResult(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath
);

internal sealed class GetLatestCreatedNewsQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetLatestCreatedNewsQuery, Result<GetLatestCreatedNewsResult>>
{
    public async Task<Result<GetLatestCreatedNewsResult>> Handle(GetLatestCreatedNewsQuery query, CancellationToken cancellationToken)
    {
        var latestNews = await dbContext
            .News
            .OrderByDescending(n => n.PublicationDate)
            .FirstOrDefaultAsync(cancellationToken);
        
        return latestNews.ToGetLatestCreatedNewsResult();
    }
}