using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.Course.CreateCourse;

public sealed record CreateCourseResponse(Guid Id);

public sealed class CreateCourseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/CreateCourse", [IgnoreAntiforgeryToken] async (
                Guid departmentID,
                string name,
                int creditHours,
                string description,
                ISender sender) =>
        {
            Guid id = Guid.NewGuid();

            var command = new CreateCourseCommand(
                id,
                departmentID,
                name,
                creditHours,
                description);

            var result = await sender.Send(command);
            return result.Match(
                onSuccess: () => Results.Created($"api/CreateCourse/{result.Value.Id}", result.Value),
                onFailure: Results.BadRequest);
        })
            .WithName("CreateCourse")
            .DisableAntiforgery()
            .Produces<CreateCourseResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Course")
            .WithDescription("Endpoint for creating a course.");
    }
}