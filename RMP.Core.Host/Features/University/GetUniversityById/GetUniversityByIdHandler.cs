using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.University.GetUniversityById;
public sealed record GetUniversityByIdQuery(Guid Id) : IQuery<Result<GetUniversityByIdResult>>;
public sealed record GetUniversityByIdResult(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber,
    string ProfilePhotoPath);

internal sealed class GetUniversityByIdQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetUniversityByIdQuery, Result<GetUniversityByIdResult>>
{
    public async Task<Result<GetUniversityByIdResult>> Handle(GetUniversityByIdQuery query, CancellationToken cancellationToken)
    {
        var university = await dbContext
            .Universities
            .FirstOrDefaultAsync(d => d.Id == query.Id, cancellationToken);

        if (university is null)
            return Result.Failure<GetUniversityByIdResult>(UniversityErrors.NotFound(query.Id));

        return university.ToGetUniversityByIdResult();
    }
}


