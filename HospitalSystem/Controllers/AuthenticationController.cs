using HospitalSystem.Infrastructure.DTOs.AuthenticationDTOs;
using HospitalSystem.Infrastructure.DTOs.UserDTOs;
using HospitalSystem.Infrastructure.DTOs.UsersTokensDTOs;
using HospitalSystem.Service.Interfaces;
using HospitalSystem.Service.Validation;
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
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IUsersTokensService _usersTokensService;

        public AuthenticationController(IUserService userService,
            ITokenService tokenService, IConfiguration configuration,
            IPasswordHasherService passwordHasher, IUsersTokensService usersTokensService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _usersTokensService = usersTokensService;
        }

        [HttpPost("login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!UsersTokensValidation.ValidateLoginRequestDto(request))
                return BadRequest("Please validate your input");

            LoginUserDto loginUserDto = await _userService.FindAsync(request.Username);

            if (loginUserDto is null)
                return Unauthorized("Invalid credentials");

            if (string.IsNullOrEmpty(loginUserDto.PasswordHash) ||
                !_passwordHasher.Verify(request.Password, loginUserDto.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateJwtToken(loginUserDto, _configuration);

            if (token is null)
                return StatusCode(500, "JWT key missing from Key Vault");

            string refreshToken = _tokenService.GenerateRefreshToken();

            bool? loginResult = await _usersTokensService.LoginAsync(loginUserDto.UserId, refreshToken, DateTime.UtcNow.AddDays(7));

            if(loginResult is null)
                return Unauthorized("Invalid credentials");

            if (loginResult == false)
                return StatusCode(500, "An error occurred while logging in");

            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            });
        }

        [HttpPost("refresh", Name = "Refresh")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto request)
        {
            if (!UsersTokensValidation.ValidateRefreshRequestDto(request))
                return BadRequest("Please validate your input");

            LoginUserDto loginUserDto = await _userService.FindAsync(request.Username);

            if (loginUserDto is null)
                return Unauthorized("Invalid refresh request");

            TokenForUserDto tokenData
                = await _usersTokensService.GetTokenDataForUserAsync(loginUserDto.UserId);

            if (tokenData is null)
                return Unauthorized("Invalid refresh request");

            if (tokenData.RevokedAt is not null)
                return Unauthorized("Refresh token is revoked");

            if (tokenData.ExpiresAt is null || tokenData.ExpiresAt <= DateTime.UtcNow)
                return Unauthorized("Refresh token expired");

            bool refreshValid = _passwordHasher.Verify(request.RefreshToken, tokenData.Hash);
            
            if (!refreshValid)
            {
                //_Logger.Log(HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                //    user.UserID.ToString(), "Invalid refresh token attempt.");
                return Unauthorized("Invalid refresh token");
            }

            var token = _tokenService.GenerateJwtToken(loginUserDto, _configuration);
            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Rotation: replace refresh token
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            bool? refreshResult = await _usersTokensService.RefreshAsync(loginUserDto.UserId, newRefreshToken, DateTime.UtcNow.AddDays(7));

            if(refreshResult is null)
                return Unauthorized("Invalid refresh request");

            if (refreshResult == false)
                return StatusCode(500, "An error occurred during refresh");

            return Ok(new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("logout", Name = "Logout")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task <IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            if (!UsersTokensValidation.ValidateLogoutRequestDto(request))
                return BadRequest("Please validate your input");

            LoginUserDto loginUserDto = await _userService.FindAsync(request.Username);

            if (loginUserDto is null)
                return Ok(); // Do not reveal if user exists

            
            var storedHash = await _usersTokensService.GetRefreshTokenHashForUserAsync(loginUserDto.UserId);
            
            if (string.IsNullOrEmpty(storedHash) ||
                !_passwordHasher.Verify(request.RefreshToken, storedHash))
                return Ok();

            bool? logoutResult = await _usersTokensService.LogoutAsync(loginUserDto.UserId, DateTime.UtcNow);

            if (logoutResult is null)
                return Unauthorized();

            if (logoutResult == false)
                return StatusCode(500, "An error occurred while logging out");

            return Ok("Logged out successfully");
        }
    }
}
