using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Mapper;

namespace RMP.Host.Features.Department.UpdateDepartment;
public sealed record UpdateDepartmentCommand(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber) : ICommand<Result<UpdateDepartmentResult>>;
public sealed record UpdateDepartmentResult(bool IsSuccess);

public sealed class UpdateDepartmentCommandValidator :
    AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required!");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required!");
        RuleFor(x => x.StaffNumber)
            .GreaterThan(0)
            .WithMessage("Staff number must be greater than zero.");
        RuleFor(x => x.StudentsNumber)
            .GreaterThan(0)
            .WithMessage("Students number must be greater than zero.");
        RuleFor(x => x.CoursesNumber)
            .GreaterThan(0)
            .WithMessage("Courses number must be greater than zero.");
    }
}

internal sealed class UpdateDepartmentCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<UpdateDepartmentCommand, Result<UpdateDepartmentResult>>
{
    public async Task<Result<UpdateDepartmentResult>> Handle(UpdateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var department = await dbContext
            .Departments
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

        if (department is null)
            return Result.Failure<UpdateDepartmentResult>(DepartmentErrors.NotFound(command.Id));
        
        command.ToDepartmentEntity(department);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new UpdateDepartmentResult(true);
    }
}