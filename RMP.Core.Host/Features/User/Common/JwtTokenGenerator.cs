using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RMP.Core.Host.Entities.Identity;

namespace RMP.Core.Host.Features.User.Common;

public class JwtTokenGenerator(IConfiguration configuration) : ITokenGenerator
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateToken(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = configuration["Jwt:Key"];
        var key = Encoding.ASCII.GetBytes(secretKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            }),
            Expires = DateTime.UtcNow.AddHours(5), 
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
