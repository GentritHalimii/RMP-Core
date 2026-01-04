using Carter;
using MediatR;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.News.GetRecentNewsCount;

public sealed class GetRecentNewsCountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetRecentNewsCount", async (ISender sender) =>
            {
                var result = await sender.Send(new GetRecentNewsCountQuery());

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetRecentNewsCount")
            .Produces<IEnumerable<DayNewsCountResult>>(StatusCodes.Status200OK) 
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Recent News Count")
            .WithDescription("Fetches the number of news items created for recent days.");
    }
}