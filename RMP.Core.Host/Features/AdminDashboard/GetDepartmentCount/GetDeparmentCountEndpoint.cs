
using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.AdminDashboard.GetDepartmentCount;

public sealed record GetDepartmentCountResponse(int Count);

public sealed class GetDepartmentCountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        
        app.MapGet("api/AdminDashboard/DepartmentCount", async (ISender sender) =>
            {
                var result = await sender.Send(new GetDepartmentCountQuery());

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetDepartmentCount")
            .Produces<GetDepartmentCountResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Department Count")
            .WithDescription("Retrieve the total number of departments.");
    }
}