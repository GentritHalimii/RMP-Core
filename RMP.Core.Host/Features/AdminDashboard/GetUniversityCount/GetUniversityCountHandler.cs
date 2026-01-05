
using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;

namespace RMP.Core.Host.Features.AdminDashboard.GetUniversityCount;


public sealed record GetUniversityCountQuery() : IQuery<Result<int>>;

internal sealed class GetUniversityCountQueryHandler(ApplicationDbContext dbContext) : 
    IQueryHandler<GetUniversityCountQuery, Result<int>>
{
    public async Task<Result<int>> Handle(GetUniversityCountQuery query, CancellationToken cancellationToken)
    {
      
        var universityCount = await dbContext.Universities.CountAsync(cancellationToken);
        
        return Result.Success(universityCount);
    }
}