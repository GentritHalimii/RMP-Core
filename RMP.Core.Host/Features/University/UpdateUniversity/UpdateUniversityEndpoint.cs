using Carter;
using MediatR;
using RMP.Host.Extensions;
using RMP.Host.Mapper;

namespace RMP.Host.Features.University.UpdateUniversity;

public sealed record UpdateUniversityRequest(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber);
public sealed record UpdateUniversityResponse(bool IsSuccess);

public sealed class UpdateUniversityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/UpdateUniversity", async (UpdateUniversityRequest request, ISender sender) =>
            {
                var command = request.ToUpdateUniversityCommand();

                var result = await sender.Send(command);
                return result.Match(
                    onSuccess: () => Results.Ok(result.IsSuccess),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("UpdateUniversity")
            .Produces<UpdateUniversityResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update University")
            .WithDescription("Update University");
    }
}