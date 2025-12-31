using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Professor.GetProfessors;

public sealed record GetProfessorsQuery() : IQuery<Result<IEnumerable<GetProfessorsResult>>>;

public sealed record GetProfessorsResult(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Education,
    string Role,
    string ProfilePhotoPath);

internal sealed class GetProfessorsQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetProfessorsQuery, Result<IEnumerable<GetProfessorsResult>>>
{
    public async Task<Result<IEnumerable<GetProfessorsResult>>> Handle(GetProfessorsQuery query, CancellationToken cancellationToken)
    {
        var professors = await dbContext.Professors
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var results = professors.Select(u => u.ToGetProfessorsResult());

        return Result.Success(results);
    }
}
