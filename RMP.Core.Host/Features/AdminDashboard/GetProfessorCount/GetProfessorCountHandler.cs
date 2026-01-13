using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;

namespace RMP.Core.Host.Features.AdminDashboard.GetProfessorCount;

public sealed record GetProfessorCountQuery() : IQuery<Result<int>>;

internal sealed class GetProfessorCountQueryHandler(ApplicationDbContext dbContext) : 
    IQueryHandler<GetProfessorCountQuery, Result<int>>
{
    public async Task<Result<int>> Handle(GetProfessorCountQuery query, CancellationToken cancellationToken)
    {
      
        var professorCount = await dbContext.Professors.CountAsync(cancellationToken);
        
        return Result.Success(professorCount);
    }
}