using Carter;
using MediatR;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Features.Professor.GetProfessors;
using RMP.Core.Host.Mapper;


namespace RMP.Core.Host.Features.Professor.GetProfessors;

public sealed record GetProfessorsResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Education,
    string Role,
    string ProfilePhotoPath);

public sealed class GetProfessorsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/Professors", async (ISender sender) =>
            {
                var result = await sender.Send(new GetProfessorsQuery());

                return result.Match(
                    onSuccess: () =>
                    {
                        var professors = result.Value.Select(u => u.ToGetProfessorsResponse());
                        return Results.Ok(professors);
                    },
                    onFailure: error => { return Results.BadRequest(error); });
            })
            .WithName("GetProfessors")
            .Produces<IEnumerable<GetProfessorsResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Professors")
            .WithDescription("Get Professors");
    }
}