using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Features.Proffesor;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Professor.UpdateProfessor;
public sealed record UpdateProfessorCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Education,
    string Role) : ICommand<Result<UpdateProfessorResult>>;
public sealed record UpdateProfessorResult(bool IsSuccess);

public sealed class UpdateProfessorCommandValidator :
    AbstractValidator<UpdateProfessorCommand>
{
    public UpdateProfessorCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required!");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required!");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required!");
        RuleFor(x => x.Education)
            .NotEmpty()
            .WithMessage("Education is required!");
        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required!");
    }
}

internal sealed class UpdateProfessorCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<UpdateProfessorCommand, Result<UpdateProfessorResult>>
{
    public async Task<Result<UpdateProfessorResult>> Handle(UpdateProfessorCommand command, CancellationToken cancellationToken)
    {
        var professor = await dbContext
            .Professors
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

        if (professor is null)
            return Result.Failure<UpdateProfessorResult>(ProfessorErrors.NotFound(command.Id));
        
        command.ToProfessorEntity(professor);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new UpdateProfessorResult(true);
    }
}