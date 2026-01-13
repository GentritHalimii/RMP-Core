using Carter;
using MediatR;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.AdminDashboard.GetStudentCount;

public sealed record GetStudentCountResponse(int Count);

public sealed class GetStudentCountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        
        app.MapGet("api/AdminDashboard/StudentCount", async (ISender sender) =>
            {
                var result = await sender.Send(new GetStudentCountQuery());

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetStudentCount")
            .Produces<GetStudentCountResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Student Count")
            .WithDescription("Retrieve the total number of students.");
    }
}