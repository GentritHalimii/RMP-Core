using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.Department.CreateDepartment;

public sealed record CreateDepartmentResponse(Guid Id);

public sealed class CreateDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/CreateDepartment", [IgnoreAntiforgeryToken] async (
                string name,
                Guid universityId,
                int establishedYear,
                string description,
                int staffNumber,
                int studentsNumber,
                int coursesNumber,
                ISender sender) =>
            {
                Guid id = Guid.NewGuid();
                
                var command = new CreateDepartmentCommand(
                    id,
                    name,
                    universityId,
                    establishedYear,
                    description,
                    staffNumber,
                    studentsNumber,
                    coursesNumber);

                var result = await sender.Send(command);
                return result.Match(
                    onSuccess: () => Results.Created($"api/CreateDepartment/{result.Value.Id}", result.Value),
                    onFailure: Results.BadRequest);
            })
            .WithName("CreateDepartment")
            .DisableAntiforgery()
            .Produces<CreateDepartmentResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Department")
            .WithDescription("Endpoint for creating a department.");
    }
}