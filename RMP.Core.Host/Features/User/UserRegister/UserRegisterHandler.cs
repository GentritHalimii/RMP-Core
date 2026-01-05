using FluentValidation;
using Microsoft.AspNetCore.Identity;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Entities.Identity;
using RMP.Host.Features.User.Common;

namespace RMP.Host.Features.User.UserRegister;

public sealed record RegisterUserCommand(
    string UserName,
    string Email,
    string Name,
    string Surname,
    Guid UniversityId,
    Guid DepartmentId,
    int Grade,
    string Password,
    string ProfilePhotoPath) : ICommand<Result<UserRegisterResult>>;

public sealed record UserRegisterResult(int Id);

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("User name is required!");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Valid email is required!");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required!");

        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage("Surname is required!");

        RuleFor(x => x.UniversityId)
            .NotEmpty()
            .WithMessage("University is required!");

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("Department is required!");
    }
}


internal sealed class RegisterUserCommandHandler(ApplicationDbContext dbContext, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager) 
    : ICommandHandler<RegisterUserCommand, Result<UserRegisterResult>>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    public async Task<Result<UserRegisterResult>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(command.Email);
        if (existingUser is not null)
            return Result.Failure<UserRegisterResult>(UserErrors.AlreadyExist(command.Email));
        
        var user = new UserEntity
        {
            UserName = command.UserName,
            Email = command.Email,
            Name = command.Name,
            Surname = command.Surname,
            UniversityId = command.UniversityId,
            DepartmentId = command.DepartmentId,
            Grade = command.Grade,
            ProfilePhotoPath = command.ProfilePhotoPath
        };
        
        var result = await userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
            return Result.Failure<UserRegisterResult>(UserErrors.RegistrationFailed(result.Errors.First().Code, result.Errors.First().Description));
        
        await userManager.AddToRoleAsync(user, UserRoleType.Student.ToString("G"));
        await signInManager.SignInAsync(user, isPersistent: false);

        return Result.Success(new UserRegisterResult(user.Id));
    }
}
