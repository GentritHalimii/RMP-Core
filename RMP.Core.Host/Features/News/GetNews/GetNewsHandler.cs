using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.News.GetNews;

public sealed record GetNewsQuery() : IQuery<Result<IEnumerable<GetNewsResult>>>;

public sealed record GetNewsResult(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath);

internal sealed class GetNewsQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetNewsQuery, Result<IEnumerable<GetNewsResult>>>
{
    public async Task<Result<IEnumerable<GetNewsResult>>> Handle(GetNewsQuery query, CancellationToken cancellationToken)
    {
        
        var news = await dbContext.News
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        var results = news.Select(n => n.ToGetNewsResult());

        return Result.Success(results);
    }
}