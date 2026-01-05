using Microsoft.EntityFrameworkCore;
using RMP.Host.Entities.Identity;

namespace RMP.Host.Features.User.Common;

public static class AuthenticationHelper
{
    public static async Task<string> AuthenticateUser(
        UserEntity user,
        DbContext dbContext,
        ITokenGenerator tokenGenerator)
    {
        var userStored = await dbContext.Set<UserEntity>()
            .FirstOrDefaultAsync(u => u.Email == user.Email);

        if (userStored is null)
            return null;
        
        return tokenGenerator.GenerateToken(userStored);;
    }
}