using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.User.GetUserById;

public sealed record GetUserByIdResponse(
    int Id,
    string Name,
    string Surname,
    Guid UniversityId,
    Guid DepartmentId,
    int Grade,
    string ProfilePhotoPath);

public sealed class GetUserByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/GetUserById/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetUserByIdQuery(id));

                return result.Match(
                    onSuccess: () => Results.Ok(result.Value.ToGetUserByIdResponse()),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName("GetUserById")
            .Produces<GetUserByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get User by Id")
            .WithDescription("Get User by Id");
    }
}