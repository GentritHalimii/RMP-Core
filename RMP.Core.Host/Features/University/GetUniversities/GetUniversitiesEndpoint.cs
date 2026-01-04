using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.University.GetUniversities;

public sealed record GetUniversitiesResponse(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber,
    string ProfilePhotoPath);

public sealed class GetUniversitiesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/Universities", async (ISender sender) =>
            {
                var result = await sender.Send(new GetUniversitiesQuery());

                return result.Match(
                    onSuccess: () =>
                    {
                        var universities = result.Value.Select(u => u.ToGetUniversitiesResponse());
                        return Results.Ok(universities);
                    },
                    onFailure: error => { return Results.BadRequest(error); });
            })
            .WithName("GetUniversities")
            .Produces<IEnumerable<GetUniversitiesResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Universities")
            .WithDescription("Get Universities");
    }
}