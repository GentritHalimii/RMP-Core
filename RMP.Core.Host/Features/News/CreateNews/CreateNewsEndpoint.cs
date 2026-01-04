using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RMP.Host.Extensions;

namespace RMP.Host.Features.News.CreateNews;

public sealed record CreateNewsResponse(Guid Id);

public sealed class CreateNewsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/CreateNews", [IgnoreAntiforgeryToken] async (
                string title,
                string content,
                DateTime publicationDate,
                string category,
                string profilePhotoPath,
                ISender sender) =>
            {
                Guid id = Guid.NewGuid();

                var command = new CreateNewsCommand(
                    id,
                    title,
                    content,
                    publicationDate,
                    category,
                    profilePhotoPath);

                var result = await sender.Send(command);
                return result.Match(
                    onSuccess: () => Results.Created($"api/CreateNews/{result.Value.Id}", result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("CreateNews")
            .DisableAntiforgery()
            .Produces<CreateNewsResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create News")
            .WithDescription("Endpoint for creating a news article.");
    }
}