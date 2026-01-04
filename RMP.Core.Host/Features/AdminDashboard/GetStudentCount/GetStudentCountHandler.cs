using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Features.User.Common;

namespace RMP.Core.Host.Features.AdminDashboard.GetStudentCount;


public sealed record GetStudentCountQuery() : IQuery<Result<int>>;

internal sealed class GetStudentCountQueryHandler(ApplicationDbContext dbContext) : 
    IQueryHandler<GetStudentCountQuery, Result<int>>
{
    public async Task<Result<int>> Handle(GetStudentCountQuery query, CancellationToken cancellationToken)
    {
      
        var studentCount = await dbContext.UserRoles
            .CountAsync(ur => ur.RoleId == (int)UserRoleType.Student);
        
        return Result.Success(studentCount);
    }
}