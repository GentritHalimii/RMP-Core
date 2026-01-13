using FluentValidation;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Entities;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Professor.CreateProfessor;

public sealed record CreateProfessorCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Education,
    string Role,
    Guid DepartmentId,
    string ProfilePhotoPath) : ICommand<Result<CreateProfessorResult>>;

public sealed record CreateProfessorResult(Guid Id);

public sealed class CreateProfessorCommandValidator : AbstractValidator<CreateProfessorCommand>
{
    public CreateProfessorCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required!");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required!");
    }
}

internal sealed class CreateProfessorCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<CreateProfessorCommand, Result<CreateProfessorResult>>
{
    public async Task<Result<CreateProfessorResult>> Handle(CreateProfessorCommand command, CancellationToken cancellationToken)
    {
        var professor = command.ToProfessorEntity();

        dbContext.Professors.Add(professor);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var departmentProfessor = new DepartmentProfessorEntity
        {
            ProfessorId = command.Id,
            DepartmentId = command.DepartmentId,
        };
        dbContext.DepartmentProfessors.Add(departmentProfessor);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateProfessorResult(professor.Id);
    }
}