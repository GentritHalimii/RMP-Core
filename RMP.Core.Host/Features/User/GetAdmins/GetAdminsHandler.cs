using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Features.User.Common;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.User.GetAdmins;

public sealed record GetAdminsQuery() : IQuery<Result<IEnumerable<GetAdminsResult>>>;
public sealed record GetAdminsResult(
    int Id,
    string Name,
    string Surname,
    Guid UniversityId,
    Guid DepartmentId,
    int Grade,
    string ProfilePhotoPath);

internal sealed class GetAdminsQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetAdminsQuery, Result<IEnumerable<GetAdminsResult>>>
{
    public async Task<Result<IEnumerable<GetAdminsResult>>> Handle(GetAdminsQuery query, CancellationToken cancellationToken)
    {
        var admins = await dbContext.Users
            .AsNoTracking()
            .Join(
                dbContext.UserRoles,
                user => user.Id,
                userRole => userRole.UserId,
                (user, userRole) => new { User = user, UserRole = userRole })
            .Where(u => u.UserRole.RoleId == (int)UserRoleType.Admin)
            .Select(u => u.User.ToGetAdminsResult())
            .ToListAsync(cancellationToken); 
        
        return Result.Success(admins.AsEnumerable());
    }
}


