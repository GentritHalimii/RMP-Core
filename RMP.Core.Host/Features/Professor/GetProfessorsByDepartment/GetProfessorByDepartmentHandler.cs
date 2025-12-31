using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.Professor.GetProfessorsByDepartment;

public sealed record GetProfessorsByDepartmentQuery(Guid DepartmentId) : IQuery<Result<IEnumerable<GetProfessorsByDepartmentResult>>>;

public sealed record GetProfessorsByDepartmentResult(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Education,
    string Role,
    string ProfilePhotoPath);

internal sealed class GetProfessorsByDepartmentQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetProfessorsByDepartmentQuery, Result<IEnumerable<GetProfessorsByDepartmentResult>>>
{
    public async Task<Result<IEnumerable<GetProfessorsByDepartmentResult>>> Handle(GetProfessorsByDepartmentQuery query, CancellationToken cancellationToken)
    {
        var professors = await dbContext.DepartmentProfessors
            .Where(dp => dp.DepartmentId == query.DepartmentId)
            .Select(dp => dp.Professor)
            .ToListAsync(cancellationToken: cancellationToken);

        var results = professors.Select(u => u.ToGetProfessorsByDepartmentResult());

        return Result.Success(results);
    }
}