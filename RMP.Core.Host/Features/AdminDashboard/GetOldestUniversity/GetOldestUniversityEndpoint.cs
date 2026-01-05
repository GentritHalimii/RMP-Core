
using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.AdminDashboard.GetOldestUniversity;

public sealed record GetOldestUniversityResponse(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber,
    string? ProfilePhotoPath);

public sealed class GetOldestUniversityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/AdminDashboard/OldestUniversity", async (ISender sender) =>
            {
                var result = await sender.Send(new GetOldestUniversityQuery());

                return result.Match(
                    onSuccess: () =>
                    {
                        var response = result.Value.ToGetOldestUniversityResponse();
                        return Results.Ok(response);
                    },
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetOldestUniversity")
            .Produces<GetOldestUniversityResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Oldest University")
            .WithDescription("Retrieve the oldest university based on its establishment year.");
    }
}