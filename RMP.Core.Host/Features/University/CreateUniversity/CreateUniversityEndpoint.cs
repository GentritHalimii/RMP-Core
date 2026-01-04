using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.University.CreateUniversity;

public sealed record CreateUniversityResponse(Guid Id);

public sealed class CreateUniversityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/CreateUniversity", [IgnoreAntiforgeryToken] async (
                [FromForm] string name,
                [FromForm] int establishedYear,
                [FromForm] string description,
                [FromForm] int staffNumber,
                [FromForm] int studentsNumber,
                [FromForm] int coursesNumber,
                IFormFile? file,
                ISender sender) =>
            {
                Guid id = Guid.NewGuid();
                string? photoPath = null;
                if (file is { Length: > 0 })
                    photoPath = FileUploadHelper.SaveProfilePhoto(file);

                var command = new CreateUniversityCommand(
                    id,
                    name,
                    establishedYear,
                    description,
                    staffNumber,
                    studentsNumber,
                    coursesNumber,
                    photoPath ?? string.Empty);

                var result = await sender.Send(command);
                return result.Match(
                    onSuccess: () => Results.Created($"api/CreateUniversity/{result.Value.Id}", result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("CreateUniversity")
            .DisableAntiforgery()
            .Produces<CreateUniversityResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create University")
            .WithDescription("Endpoint for creating a university.");
    }
}