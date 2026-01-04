using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Core.Host.Mapper;

namespace RMP.Core.Host.Features.User.GetUserById;

public sealed record GetUserByIdQuery(int Id) : IQuery<Result<GetUserByIdResult>>;
public sealed record GetUserByIdResult(
    int Id,
    string Name,
    string Surname,
    Guid UniversityId,
    Guid DepartmentId,
    int Grade,
    string ProfilePhotoPath);

internal sealed class GetUserByIdQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetUserByIdQuery, Result<GetUserByIdResult>>
{
    public async Task<Result<GetUserByIdResult>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await dbContext
            .Users
            .FirstOrDefaultAsync(d => d.Id == query.Id, cancellationToken);

        if (user is null)
            return Result.Failure<GetUserByIdResult>(UserErrors.UserNotFound(query.Id));

        return user.ToGetUserByIdResult();
    }
}
