using HospitalSystem.Infrastructure.DTOs.UserDTOs;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace HospitalSystem.Service.Interfaces
{
    public interface ITokenService
    {
        public string GenerateRefreshToken();
        public JwtSecurityToken GenerateJwtToken(LoginUserDto loginUserDto, IConfiguration Configuration);
    }
}
