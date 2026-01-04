using Microsoft.EntityFrameworkCore;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Mapper;

namespace RMP.Host.Features.University.GetUniversityById;
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


