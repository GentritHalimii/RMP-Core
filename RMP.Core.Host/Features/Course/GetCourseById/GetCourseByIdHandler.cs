using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Course.GetCourseById;
public sealed record GetCourseByIdQuery(Guid Id) : IQuery<Result<GetCourseByIdResult>>;

public sealed record GetCourseByIdResult(
    Guid Id,
    string Name,
    int CreditHours,
    string Description)
;

internal sealed class GetCourseByIdQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetCourseByIdQuery, Result<GetCourseByIdResult>>
{
    public async Task<Result<GetCourseByIdResult>> Handle(GetCourseByIdQuery query, CancellationToken cancellationToken)
    {
        var course = await dbContext
            .Courses
            .FirstOrDefaultAsync(d => d.Id == query.Id, cancellationToken);

        if (course is null)
            return Result.Failure<GetCourseByIdResult>(CourseErrors.NotFound(query.Id));

        return course.ToGetCourseByIdResult();
    }
}