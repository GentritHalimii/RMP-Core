using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Entities.Identity;
using RMP.Host.Features.User.Common;

namespace RMP.Host.Features.User.UserLogin;

public sealed record LoginUserCommand(
    string Email,
    string Password,
    bool RememberMe) : ICommand<Result<UserLoginResponse>>;
    
public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Valid email is required!");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required!");
    }
}

internal sealed class LoginUserCommandHandler(
    ApplicationDbContext dbContext,
    SignInManager<UserEntity> signInManager,
    UserManager<UserEntity> userManager,
    ITokenGenerator tokenGenerator)
    : ICommandHandler<LoginUserCommand, Result<UserLoginResponse>>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<Result<UserLoginResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await dbContext.Set<UserEntity>()
            .FirstOrDefaultAsync(u => u.Email == command.Email);

        if (user is null)
            return Result.Failure<UserLoginResponse>(UserErrors.UserNotFound(user!.Id));

        var result = await signInManager.PasswordSignInAsync(user, command.Password, command.RememberMe, lockoutOnFailure: false);

        if (!result.Succeeded)
            return Result.Failure<UserLoginResponse>(UserErrors.LoginFailed());

        var token = await AuthenticationHelper.AuthenticateUser(user, dbContext, tokenGenerator);

        if (token is null)
            return Result.Failure<UserLoginResponse>(UserErrors.AuthenticationFailed());

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
        var currentUser = await userManager.GetUserAsync(claimsPrincipal);
        var role = (List<string>)await userManager.GetRolesAsync(currentUser!);
        
        return Result.Success(new UserLoginResponse(token, user.Id, user.Email!, user.DepartmentId, role));
    }
}


