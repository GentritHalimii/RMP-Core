using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;

namespace RMP.Core.Host.Features.Department.DeleteDepartment;
public sealed record DeleteDepartmentCommand(Guid Id) : ICommand<Result<DeleteDepartmentResult>>;
public sealed record DeleteDepartmentResult(bool IsSuccess);

internal sealed class DeleteDepartmentCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<DeleteDepartmentCommand, Result<DeleteDepartmentResult>>
{
    public async Task<Result<DeleteDepartmentResult>> Handle(DeleteDepartmentCommand command, CancellationToken cancellationToken)
    {
        var department = await dbContext
            .Departments
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

        if (department is null)
            return Result.Failure<DeleteDepartmentResult>(DepartmentErrors.NotFound(command.Id));

        dbContext.Departments.Remove(department);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteDepartmentResult(true);
    }
}