using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;


namespace RMP.Core.Host.Features.News.GetNews;


public sealed record GetNewsResponse(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath);


public sealed class GetNewsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/News", async (ISender sender) =>
            {
                var result = await sender.Send(new GetNewsQuery());

                return result.Match(
                    onSuccess: () =>
                    {
                        var news = result.Value.Select(n => n.ToGetNewsResponse());
                        return Results.Ok(news);
                    },
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetNews")
            .Produces<IEnumerable<GetNewsResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get News")
            .WithDescription("Retrieve all news articles.");
    }
}