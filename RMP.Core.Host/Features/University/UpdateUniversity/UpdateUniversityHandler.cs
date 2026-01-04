using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Mapper;

namespace RMP.Host.Features.University.UpdateUniversity;
public sealed record UpdateUniversityCommand(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber) : ICommand<Result<UpdateUniversityResult>>;
public sealed record UpdateUniversityResult(bool IsSuccess);

public sealed class UpdateUniversityCommandValidator :
    AbstractValidator<UpdateUniversityCommand>
{
    public UpdateUniversityCommandValidator()
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

internal sealed class UpdateUniversityCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<UpdateUniversityCommand, Result<UpdateUniversityResult>>
{
    public async Task<Result<UpdateUniversityResult>> Handle(UpdateUniversityCommand command, CancellationToken cancellationToken)
    {
        var university = await dbContext
            .Universities
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

        if (university is null)
            return Result.Failure<UpdateUniversityResult>(UniversityErrors.NotFound(command.Id));
        
        command.ToUniversityEntity(university);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new UpdateUniversityResult(true);
    }
}