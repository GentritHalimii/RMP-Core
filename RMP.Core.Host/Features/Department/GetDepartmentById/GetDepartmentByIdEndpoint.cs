using Carter;
using MediatR;
using RMP.Host.Extensions;
using RMP.Host.Mapper;

namespace RMP.Host.Features.Department.GetDepartmentById;

public sealed record GetDepartmentByIdResponse(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber);

public sealed class GetDepartmentByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetDepartmentById/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetDepartmentByIdQuery(id));

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value.ToGetDepartmentByIdResponse()),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetDepartmentById")
            .Produces<GetDepartmentByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Department by Id")
            .WithDescription("Get Department by Id");
    }
}