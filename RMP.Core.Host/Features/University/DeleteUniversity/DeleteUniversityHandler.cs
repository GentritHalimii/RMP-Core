using Microsoft.EntityFrameworkCore;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;

namespace RMP.Host.Features.University.DeleteUniversity;
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