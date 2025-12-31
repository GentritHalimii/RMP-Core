using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Features.Proffesor;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Professor.GetProfessorById;
public sealed record GetProfessorByIdQuery(Guid Id) : IQuery<Result<GetProfessorByIdResult>>;
public sealed record GetProfessorByIdResult(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Education,
    string Role,
    string ProfilePhotoPath);

internal sealed class GetProfessorByIdQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetProfessorByIdQuery, Result<GetProfessorByIdResult>>
{
    public async Task<Result<GetProfessorByIdResult>> Handle(GetProfessorByIdQuery query, CancellationToken cancellationToken)
    {
        var proffesor = await dbContext
            .Professors
            .FirstOrDefaultAsync(d => d.Id == query.Id, cancellationToken);

        if (proffesor is null)
            return Result.Failure<GetProfessorByIdResult>(ProfessorErrors.NotFound(query.Id));

        return proffesor.ToGetProfessorByIdResult();
    }
}


