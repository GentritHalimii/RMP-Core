using FluentValidation;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Department.CreateDepartment;

public sealed record CreateDepartmentCommand(
    Guid Id,
    string Name,
    Guid UniversityId,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber) : ICommand<Result<CreateDepartmentResult>>;

public sealed record CreateDepartmentResult(Guid Id);

public sealed class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required!");
    }
}

internal sealed class CreateDepartmentCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<CreateDepartmentCommand, Result<CreateDepartmentResult>>
{
    public async Task<Result<CreateDepartmentResult>> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var department = command.ToDepartmentEntity();

        dbContext.Departments.Add(department);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateDepartmentResult(department.Id);
    }
}