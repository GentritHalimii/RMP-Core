using FluentValidation;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Mapper;

namespace RMP.Host.Features.University.CreateUniversity;

public sealed record CreateUniversityCommand(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber,
    string ProfilePhotoPath) : ICommand<Result<CreateUniversityResult>>;

public sealed record CreateUniversityResult(Guid Id);

public sealed class CreateUniversityCommandValidator : AbstractValidator<CreateUniversityCommand>
{
    public CreateUniversityCommandValidator()
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

internal sealed class CreateUniversityCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<CreateUniversityCommand, Result<CreateUniversityResult>>
{
    public async Task<Result<CreateUniversityResult>> Handle(CreateUniversityCommand command, CancellationToken cancellationToken)
    {
        var university = command.ToUniversityEntity();

        dbContext.Universities.Add(university);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateUniversityResult(university.Id);
    }
}