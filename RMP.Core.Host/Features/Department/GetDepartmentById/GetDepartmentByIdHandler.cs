using Microsoft.EntityFrameworkCore;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Mapper;

namespace RMP.Host.Features.Department.GetDepartmentById;
public sealed record GetDepartmentByIdQuery(Guid Id) : IQuery<Result<GetDepartmentByIdResult>>;

public sealed record GetDepartmentByIdResult(
    Guid Id,
    string Name,
    int EstablishedYear,
    string Description,
    int StaffNumber,
    int StudentsNumber,
    int CoursesNumber)
;

internal sealed class GetDepartmentByIdQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetDepartmentByIdQuery, Result<GetDepartmentByIdResult>>
{
    public async Task<Result<GetDepartmentByIdResult>> Handle(GetDepartmentByIdQuery query, CancellationToken cancellationToken)
    {
        var department = await dbContext
            .Departments
            .FirstOrDefaultAsync(d => d.Id == query.Id, cancellationToken);

        if (department is null)
            return Result.Failure<GetDepartmentByIdResult>(DepartmentErrors.NotFound(query.Id));

        return department.ToGetDepartmentByIdResult();
    }
}