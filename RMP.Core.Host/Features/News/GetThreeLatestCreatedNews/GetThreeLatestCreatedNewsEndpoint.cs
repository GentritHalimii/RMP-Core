using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Host.Extensions;
using RMP.Host.Mapper;

namespace RMP.Host.Features.News.GetThreeLatestCreatedNews;


public sealed record GetThreeLatestCreatedNewsResponse(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath);

public sealed class GetThreeLatestCreatedNewsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetThreeLatestCreatedNews", async  ( ISender sender) =>
            {
                var result = await sender.Send(new GetThreeLatestCreatedNewsQuery());

                return result.Match(
                    onSuccess: () =>
                    {
                        var news = result.Value.Select(n => n.ToGetThreeLatestCreatedNewsResponse());
                        return Results.Ok(news);
                    },
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetThreeLatestCreatedNews")
            .Produces<List<GetThreeLatestCreatedNewsResult>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Three Latest Created News")
            .WithDescription("Fetches the three most recently created news articles.");
    }
}