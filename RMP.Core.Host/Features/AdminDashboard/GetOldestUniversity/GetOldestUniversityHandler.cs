
using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.AdminDashboard.GetOldestUniversity;

public sealed record GetOldestUniversityQuery() : IQuery<Result<GetOldestUniversityResult>>;

public sealed record GetOldestUniversityResult(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber,
    string? ProfilePhotoPath);

internal sealed class GetOldestUniversityQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetOldestUniversityQuery, Result<GetOldestUniversityResult>>
{
    public async Task<Result<GetOldestUniversityResult>> Handle(GetOldestUniversityQuery query, CancellationToken cancellationToken)
    {
        var oldestUniversityEntity = await dbContext.Universities
            .AsNoTracking()
            .OrderBy(u => u.EstablishedYear)
            .FirstOrDefaultAsync(cancellationToken);

        var result = oldestUniversityEntity.ToGetOldestUniversityResult();
        return Result.Success(result);
    }
}