using Microsoft.EntityFrameworkCore;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Mapper;

namespace RMP.Host.Features.University.GetUniversities;

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
