
using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.AdminDashboard.GetUniversityCount;

public sealed record GetUniversityCountResponse(int Count);

public sealed class GetUniversityCountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        
        app.MapGet("api/AdminDashboard/UniversityCount", async (ISender sender) =>
            {
                var result = await sender.Send(new GetUniversityCountQuery());

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: Results.BadRequest);
            })
            .WithName("GetUniversityCount")
            .Produces<GetUniversityCountResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get University Count")
            .WithDescription("Retrieve the total number of universities.");
    }
}