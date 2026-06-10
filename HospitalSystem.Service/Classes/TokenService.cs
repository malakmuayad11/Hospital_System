using HospitalSystem.Infrastructure.DTOs.UserDTOs;
using HospitalSystem.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HospitalSystem.Service.Classes
{
    public class TokenService : ITokenService
    {
        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public JwtSecurityToken GenerateJwtToken(LoginUserDto loginUserDto, IConfiguration Configuration)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loginUserDto.UserId.ToString()),
                new Claim(ClaimTypes.Role, UserService.RoleString(loginUserDto.Role)),
                new Claim("permissions", UserService.PermissionsString(loginUserDto.Permissions))
            };

            var secretKey = Configuration["JwtSigningKey"];

                if (string.IsNullOrWhiteSpace(secretKey))
                    return null;

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                return new JwtSecurityToken(
                    issuer: "HospitalSystemApi",
                    audience: "HospitalSystemApiUsers",
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds
                );
        }
    }
}
