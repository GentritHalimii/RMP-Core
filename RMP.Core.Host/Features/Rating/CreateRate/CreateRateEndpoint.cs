using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.Rating.CreateRate;

public sealed record CreateRateRequest(
    string EntityType,
    Guid EntityId,
    int UserId,
    int Overall,
    string Feedback,
    int? CommunicationSkills = null,
    int? Responsiveness = null,
    int? GradingFairness = null);

public sealed record RateResponse(
    Guid Id,
    Guid EntityId,
    int UserId,
    int Overall,
    string Feedback,
    int? CommunicationSkills,
    int? Responsiveness,
    int? GradingFairness);

public sealed class CreateRateEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/CreateRate", [IgnoreAntiforgeryToken] async (CreateRateRequest request, ISender sender) =>
            {
                var command = new CreateRateCommand(request);
                var result = await sender.Send(command);
                return result.Match(
                    onSuccess: () => Results.Created($"api/CreateRate/{result.Value.Id}", result.Value),
                    onFailure: Results.BadRequest);
            })
            .WithName("CreateRate")
            .DisableAntiforgery()
            .Produces<RateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Rate")
            .WithDescription("Endpoint for creating ratings for different entities.");
    }
}
