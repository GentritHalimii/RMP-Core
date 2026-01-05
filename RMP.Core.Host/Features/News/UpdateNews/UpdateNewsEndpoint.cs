using Carter;
using MediatR;
using RMP.Host.Extensions;
using RMP.Host.Mapper;

namespace RMP.Host.Features.News.UpdateNews;

public sealed record UpdateNewsRequest(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath);

public sealed record UpdateNewsResponse(bool IsSuccess);

public sealed class UpdateNewsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/UpdateNews", async (UpdateNewsRequest request, ISender sender) =>
            {
                var command = request.ToUpdateNewsCommand();

                var result = await sender.Send(command);
                return result.Match(
                    onSuccess: () => Results.Ok(new UpdateNewsResponse(true)),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("UpdateNews")
            .Produces<UpdateNewsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update News")
            .WithDescription("Endpoint for updating an existing news article.");
    }
}