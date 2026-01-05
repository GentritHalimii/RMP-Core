using Carter;
using MediatR;
using RMP.Host.Extensions;
using RMP.Host.Mapper;

namespace RMP.Host.Features.Department.UpdateDepartment;

public sealed record UpdateDepartmentRequest(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber);
public sealed record UpdateDepartmentResponse(bool IsSuccess);

public sealed class UpdateDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/UpdateDepartment", async (UpdateDepartmentRequest request, ISender sender) =>
            {
                var command = request.ToUpdateDepartmentCommand();

                var result = await sender.Send(command);
                return result.Match(
                    onSuccess: () => Results.Ok(result.IsSuccess),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("UpdateDepartment")
            .Produces<UpdateDepartmentResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Department")
            .WithDescription("Update Department");
    }
}