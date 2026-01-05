using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RMP.Host.Extensions;

namespace RMP.Host.Features.User.UserRegister;

public sealed record UserRegisterResponse(Guid Id);

public sealed class UserRegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/RegisterUser", async (
                [FromForm] string userName,
                [FromForm] string email,
                [FromForm] string name,
                [FromForm] string surname,
                [FromForm] Guid universityId,
                [FromForm] Guid departmentId,
                [FromForm] int grade,
                [FromForm] string password,
                [FromForm] IFormFile? file,
                ISender sender) =>
            {
                string? photoPath = null;

                if (file is { Length: > 0 })
                    photoPath = FileUploadHelper.SaveProfilePhoto(file);

                var command = new RegisterUserCommand(
                    userName,
                    email,
                    name,
                    surname,
                    universityId,
                    departmentId,
                    grade,
                    password,
                    photoPath ?? string.Empty);

                var result = await sender.Send(command);

                return result.Match(
                    onSuccess: () => Results.Created($"api/RegisterUser/{result.Value.Id}", result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("RegisterUser")
            .DisableAntiforgery()
            .Produces<UserRegisterResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Register User")
            .WithDescription("Endpoint for registering a new user.");
    }
}
