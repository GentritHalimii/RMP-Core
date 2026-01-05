using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Mapper;

namespace RMP.Host.Features.News.GetNewsById;

public sealed record GetNewsByIdQuery(Guid Id) : IQuery<Result<GetNewsByIdResult>>;

public sealed record GetNewsByIdResult(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath
);

internal sealed class GetNewsByIdQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetNewsByIdQuery, Result<GetNewsByIdResult>>
{
    public async Task<Result<GetNewsByIdResult>> Handle(GetNewsByIdQuery query, CancellationToken cancellationToken)
    {
        var news = await dbContext
            .News
            .FirstOrDefaultAsync(n => n.Id == query.Id, cancellationToken);

        if (news is null)
            return Result.Failure<GetNewsByIdResult>(NewsErrors.NotFound(query.Id));

        return news.ToGetNewsByIdResult();
    }
}