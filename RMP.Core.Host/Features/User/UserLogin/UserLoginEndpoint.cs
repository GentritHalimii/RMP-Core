using Carter;
using MediatR;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.User.UserLogin;

public sealed record UserLoginRequest(string Email, string Password, bool RememberMe);
public sealed record UserLoginResponse(string Token, int UserId, string Email, Guid DepartmentId, IEnumerable<string> Roles);

public sealed class UserLoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/Login", async (
                UserLoginRequest userLoginRequest,
                ISender sender) =>
            {
                var command = new LoginUserCommand(userLoginRequest.Email, userLoginRequest.Password, userLoginRequest.RememberMe);

                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("LoginUser")
            .DisableAntiforgery()
            .Produces<UserLoginResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("User Login")
            .WithDescription("Endpoint for authenticating a user.");
    }
}