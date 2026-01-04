using Carter;
using MediatR;
using RMP.Core.Host.Extensions;

namespace RMP.Core.Host.Features.User.GetAdmins;

public sealed record GetAdminsResponse(
    int Id,
    string Name,
    string Surname,
    Guid UniversityId,
    Guid DepartmentId,
    int Grade,
    string ProfilePhotoPath);

public sealed class GetAdminsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetAdmins", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAdminsQuery());

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetAdmins")
            .Produces<IEnumerable<GetAdminsResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all Admins")
            .WithDescription("Get all Admins");

    }
}