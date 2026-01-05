using Carter;
using MediatR;
using RMP.Host.Extensions;
using RMP.Host.Mapper;

namespace RMP.Host.Features.Department.GetDepartments;

public sealed record GetDepartmentsResponse(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber);

public sealed class GetDepartmentsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/Departments", async (ISender sender) =>
            {
                var result = await sender.Send(new GetDepartmentsQuery());

                return result.Match(
                    onSuccess: () =>
                    {
                        var departments = result.Value.Select(u => u.ToGetDepartmentsResponse());
                        return Results.Ok(departments);
                    },
                    onFailure: error => { return Results.BadRequest(error); });
            })
            .WithName("GetDepartments")
            .Produces<IEnumerable<GetDepartmentsResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Departments")
            .WithDescription("Get Departments");
    }
}