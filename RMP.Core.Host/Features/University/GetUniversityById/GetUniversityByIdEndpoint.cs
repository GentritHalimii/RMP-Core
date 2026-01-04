using Carter;
using MediatR;
using RMP.Host.Extensions;
using RMP.Host.Mapper;

namespace RMP.Host.Features.University.GetUniversityById;

public sealed record GetUniversityByIdResponse(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber,
    string ProfilePhotoPath);

public sealed class GetUniversityByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetUniversityById/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetUniversityByIdQuery(id));

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value.ToGetUniversityByIdResponse()),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetUniversityById")
            .Produces<GetUniversityByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get University by Id")
            .WithDescription("Get University by Id");
    }
}

