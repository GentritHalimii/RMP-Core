using FluentValidation;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Mapper;

namespace RMP.Host.Features.Department.CreateDepartment;

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