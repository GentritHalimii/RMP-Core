using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;

namespace RMP.Core.Host.Features.AdminDashboard.GetDepartmentCount;

public sealed record GetDepartmentCountQuery() : IQuery<Result<int>>;

internal sealed class GetDepartmentCountQueryHandler(ApplicationDbContext dbContext) : 
    IQueryHandler<GetDepartmentCountQuery, Result<int>>
{
    public async Task<Result<int>> Handle(GetDepartmentCountQuery query, CancellationToken cancellationToken)
    {
      
        var departmentCount = await dbContext.Departments.CountAsync(cancellationToken);
        
        return Result.Success(departmentCount);
    }
}