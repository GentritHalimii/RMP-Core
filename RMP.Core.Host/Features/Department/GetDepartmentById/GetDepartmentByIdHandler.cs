using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Department.GetDepartmentById;
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