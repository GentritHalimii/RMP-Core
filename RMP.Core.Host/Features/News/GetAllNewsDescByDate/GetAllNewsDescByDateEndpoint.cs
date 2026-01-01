using Carter;
using MediatR;
using RMP.Host.Extensions;
using RMP.Host.Mapper;

namespace RMP.Host.Features.News.GetAllNewsDescByDate;

public sealed record GetAllNewsDescByDateResponse(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath);

public sealed class GetAllNewsDescByDateEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetAllNewsDescByDate", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllNewsDescByDateQuery());

                return result.Match(
                    onSuccess: () =>
                    {
                        var news = result.Value.Select(n => n.ToGetAllNewsDescByDateResponse());
                        return Results.Ok(news);
                    },
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetAllNewsDescByDate")
            .Produces<IEnumerable<GetAllNewsDescByDateResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get All News by Date Descending")
            .WithDescription("Retrieve all news articles sorted by publication date in descending order.");
    }
}