using Microsoft.EntityFrameworkCore;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Features.User.Common;
using RMP.Host.Mapper;

namespace RMP.Host.Features.User.GetStudents;

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