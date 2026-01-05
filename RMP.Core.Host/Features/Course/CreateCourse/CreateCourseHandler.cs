using FluentValidation;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Course.CreateCourse;

public sealed record CreateCourseCommand(
    Guid Id,
    Guid DepartmentID,
    string Name,
    int CreditHours,
    string Description) : ICommand<Result<CreateCourseResult>>;

public sealed record CreateCourseResult(Guid Id);

public sealed class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required!");
        RuleFor(x => x.DepartmentID)
            .NotEmpty()
            .WithMessage("DepartmentID is required!");
        RuleFor(x => x.CreditHours)
            .NotEmpty()
            .WithMessage("CreditHours is required!");
    }
}

internal sealed class CreateCourseCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<CreateCourseCommand, Result<CreateCourseResult>>
{
    public async Task<Result<CreateCourseResult>> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
    {
        var course = command.ToCourseEntity();

        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateCourseResult(course.Id);
    }
}