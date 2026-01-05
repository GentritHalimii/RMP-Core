using RMP.Host.Entities.Identity;

namespace RMP.Host.Features.User.Common;

public interface ITokenGenerator
{
    string GenerateToken(UserEntity user);
}