using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.University.GetUniversities;

public sealed record GetUniversitiesQuery() : IQuery<Result<IEnumerable<GetUniversitiesResult>>>;

public sealed record GetUniversitiesResult(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber,
    string ProfilePhotoPath);

internal sealed class GetUniversitiesQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetUniversitiesQuery, Result<IEnumerable<GetUniversitiesResult>>>
{
    public async Task<Result<IEnumerable<GetUniversitiesResult>>> Handle(GetUniversitiesQuery query, CancellationToken cancellationToken)
    {
        var universities = await dbContext.Universities
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var results = universities.Select(u => u.ToGetUniversitiesResult());

        return Result.Success(results);
    }
}
