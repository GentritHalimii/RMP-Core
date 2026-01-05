using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Host.Extensions;
using RMP.Host.Mapper;

namespace RMP.Host.Features.News.GetNewsById;

public sealed record GetNewsByIdResponse(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath);

public sealed class GetNewsByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetNewsById/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetNewsByIdQuery(id));

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value.ToGetNewsByIdResponse()),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetNewsById")
            .Produces<GetNewsByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get News by Id")
            .WithDescription("Get News by Id");
    }
}