using Microsoft.EntityFrameworkCore;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Mapper;

namespace RMP.Host.Features.Department.GetDepartments;

public sealed record GetDepartmentsQuery() : IQuery<Result<IEnumerable<GetDepartmentsResult>>>;

public sealed record GetDepartmentsResult(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber);

internal sealed class GetDepartmentsQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetDepartmentsQuery, Result<IEnumerable<GetDepartmentsResult>>>
{
    public async Task<Result<IEnumerable<GetDepartmentsResult>>> Handle(GetDepartmentsQuery query, CancellationToken cancellationToken)
    {
        var departments = await dbContext.Departments
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var results = departments.Select(u => u.ToGetDepartmentsResult());

        return Result.Success(results);
    }
}