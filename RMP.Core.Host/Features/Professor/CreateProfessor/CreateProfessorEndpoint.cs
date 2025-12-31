using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.Professor.CreateProfessor;

public sealed record CreateProfessorResponse(Guid Id);

public sealed class CreateProfessorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/CreateProfessor", [IgnoreAntiforgeryToken] async (
                [FromForm] string firstName,
                [FromForm] string lastName,
                [FromForm] string username,
                [FromForm] string email,
                [FromForm] string education,
                [FromForm] string role,
                IFormFile? file,
                Guid departmentId,
                ISender sender) =>
            {
                Guid id = Guid.NewGuid();
                string? photoPath = null;
                if (file is { Length: > 0 })
                    photoPath = FileUploadHelper.SaveProfilePhoto(file);

                var command = new CreateProfessorCommand(
                    id,
                    firstName,
                    lastName,
                    username,
                    email,
                    education,
                    role,
                    departmentId,
                    photoPath ?? string.Empty);

                var result = await sender.Send(command);
                return result.Match(
                    onSuccess: () => Results.Created($"api/CreateProfessor/{result.Value.Id}", result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("CreateProfessor")
            .DisableAntiforgery()
            .Produces<CreateProfessorResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Professor")
            .WithDescription("Endpoint for creating a professor.");
    }
}