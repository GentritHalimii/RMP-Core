using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;

namespace RMP.Host.Features.News.DeleteNews;

public sealed record DeleteNewsCommand(Guid Id) : ICommand<Result<DeleteNewsResult>>;
public sealed record DeleteNewsResult(bool IsSuccess);

internal sealed class DeleteNewsCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<DeleteNewsCommand, Result<DeleteNewsResult>>
{
    public async Task<Result<DeleteNewsResult>> Handle(DeleteNewsCommand command, CancellationToken cancellationToken)
    {
        var news = await dbContext
            .News
            .FirstOrDefaultAsync(n => n.Id == command.Id, cancellationToken);

        if (news is null)
            return Result.Failure<DeleteNewsResult>(NewsErrors.NotFound(command.Id));

        dbContext.News.Remove(news);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteNewsResult(true);
    }
}