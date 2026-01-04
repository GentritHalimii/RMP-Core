using RMP.Core.Host.Entities.Identity;

namespace RMP.Core.Host.Features.User.Common;

public interface ITokenGenerator
{
    string GenerateToken(UserEntity user);
}