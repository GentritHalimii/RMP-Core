using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;

namespace RMP.Core.Host.Features.University.DeleteUniversity;
public sealed record DeleteUniversityCommand(Guid Id) : ICommand<Result<DeleteUniversityResult>>;
public sealed record DeleteUniversityResult(bool IsSuccess);

internal sealed class DeleteUniversityCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<DeleteUniversityCommand, Result<DeleteUniversityResult>>
{
    public async Task<Result<DeleteUniversityResult>> Handle(DeleteUniversityCommand command, CancellationToken cancellationToken)
    {
        var university = await dbContext
            .Universities
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

        if (university is null)
            return Result.Failure<DeleteUniversityResult>(UniversityErrors.NotFound(command.Id));

        dbContext.Universities.Remove(university);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteUniversityResult(true);
    }
}