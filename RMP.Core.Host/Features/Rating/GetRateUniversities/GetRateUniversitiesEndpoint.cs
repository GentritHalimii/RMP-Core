using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Rating.GetRateUniversities;

public sealed record GetRateUniversitiesResponse(
    Guid Id,
    Guid UniversityId,
    int UserId,
    int Overall,
    string Feedback);

public sealed class GetRateUniversitiesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/RateUniversities", async (ISender sender) =>
            {
                var result = await sender.Send(new GetRateUniversitiesQuery());

                return result.Match(
                    onSuccess: () =>
                    {
                        var rateUniversities = result.Value.Select(u => u.ToGetRateUniversitiesResponse());
                        return Results.Ok(rateUniversities);
                    },
                    onFailure: error => { return Results.BadRequest(error); });
            })
            .WithName("GetRateUniversities")
            .Produces<IEnumerable<GetRateUniversitiesResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get RateUniversities")
            .WithDescription("Get RateUniversities");
    }
}