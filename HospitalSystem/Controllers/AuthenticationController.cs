using HospitalSystem.Infrastructure.DTOs.AuthenticationDTOs;
using HospitalSystem.Infrastructure.DTOs.UserDTOs;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace HospitalSystem.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IUserService userService, ITokenService tokenService, IConfiguration configuration)
        {
            _userService = userService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            LoginUserDto loginUserDto = await _userService.FindAsync(request.Username);

            if (loginUserDto is null)
                return Unauthorized("Invalid credentials");

            if (string.IsNullOrEmpty(loginUserDto.PasswordHash) ||
                !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, loginUserDto.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateJwtToken(loginUserDto, _configuration);

            if (token is null)
                return StatusCode(500, "JWT key missing from Key Vault");

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
