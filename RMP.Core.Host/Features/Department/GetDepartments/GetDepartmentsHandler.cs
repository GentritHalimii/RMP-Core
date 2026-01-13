using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Department.GetDepartments;

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