using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.News.GetLatestCreatedNews;

public sealed record GetLatestCreatedNewsResponse(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath
);

public sealed class GetLatestCreatedNewsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetLatestCreatedNews", async (ISender sender) =>
            {
                var result = await sender.Send(new GetLatestCreatedNewsQuery());

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value.ToGetLatestCreatedNewsResponse()),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetLatestCreatedNews")
            .Produces<GetLatestCreatedNewsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Latest Created News")
            .WithDescription("Get the latest created news article from the system");
    }
}