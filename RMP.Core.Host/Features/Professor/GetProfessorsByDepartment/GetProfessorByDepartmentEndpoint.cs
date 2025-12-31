using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Professor.GetProfessorsByDepartment;

public sealed record GetProfessorsByDepartmentResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Education,
    string Role,
    string ProfilePhotoPath);

public sealed class GetProfessorsByDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/Professors/department/{departmentId}", async (ISender sender, Guid departmentId) =>
            {
                var result = await sender.Send(new GetProfessorsByDepartmentQuery(departmentId));

                return result.Match(
                    onSuccess: () =>
                    {
                        var professors = result.Value.Select(u => u.ToGetProfessorsByDepartmentResponse());
                        return Results.Ok(professors);
                    },
                    onFailure: error => { return Results.BadRequest(error); });
            })
            .WithName("GetProfessorsByDepartment")
            .Produces<IEnumerable<GetProfessorsByDepartmentResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Professors")
            .WithDescription("Get Professors");
    }
}
