using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Features.User.Common;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.User.GetStudents;

public sealed record GetStudentsQuery() : IQuery<Result<IEnumerable<GetStudentsResult>>>;
public sealed record GetStudentsResult(
    int Id,
    string Name,
    string Surname,
    Guid UniversityId,
    Guid DepartmentId,
    int Grade,
    string ProfilePhotoPath);

internal sealed class GetStudentsQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetStudentsQuery, Result<IEnumerable<GetStudentsResult>>>
{
    public async Task<Result<IEnumerable<GetStudentsResult>>> Handle(GetStudentsQuery query, CancellationToken cancellationToken)
    {
        var students = await dbContext.Users
            .AsNoTracking()
            .Join(
                dbContext.UserRoles,
                user => user.Id,
                userRole => userRole.UserId,
                (user, userRole) => new { User = user, UserRole = userRole })
            .Where(u => u.UserRole.RoleId == (int)UserRoleType.Student)
            .Select(u => u.User.ToGetStudentsResult())
            .ToListAsync(cancellationToken);

        return Result.Success(students.AsEnumerable());
    }
}