using HospitalSystem.API.Validation;
using HospitalSystem.Infrastructure.DTOs.Users;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace HospitalSystem.API.Controllers
{
    [EnableRateLimiting("CriticalOpsLimiter")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILoggerService _loggerService;

        public UsersController(IUserService UserService, IAuthorizationService authorizationService
            , ILoggerService loggerService)
        {
            _userService = UserService;
            _authorizationService = authorizationService;
            _loggerService = loggerService;
        }

        [Authorize(Policy = "ManageUsers")]
        [HttpGet(Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            List<UserDto> users = await _userService.GetAllUsersAsync();

            if (users == null)
                return NotFound("Users are not found");

            return Ok(users);
        }

        [Authorize(Policy = "ManageUsers")]
        [HttpGet("count", Name = "GetUsersCount")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<int>> GetUsersCountAsync()
        {
            int usersCount = await _userService.GetUsersCountAsync();

            if (usersCount == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while counting users." });

            return Ok(usersCount);
        }

        [Authorize(Policy = "ManageUsers")]
        [HttpPost(Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<UserDto>> AddNewUserAsync(AddUserDto addUserDto)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!UserValidation.ValidateAddUserInput(addUserDto))
                return BadRequest("Please validate your input.");

            int? userId = await _userService.AddUserAsync(addUserDto);

            if (userId is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new user." });

            _loggerService.Log($"Added new user with id: {userId}.", ip, userID);
            return CreatedAtRoute("FindUserById", new { userId = userId }, addUserDto);
        }

        [HttpPut(Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!UserValidation.ValidateUpdateUserInput(updateUserDto))
                return BadRequest("Please validate your input.");

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(
                User,
                updateUserDto.UserId,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); 

            bool? result = await _userService.UpdateUserAsync(updateUserDto);

            if (result is null)
                return NotFound($"User with id: {updateUserDto.UserId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the user." });

            _loggerService.Log($"Updated user with id: {updateUserDto.UserId}.", ip, userId);
            return Ok("User is updated successfully.");
        }

        [HttpPatch("changePassword", Name = "changePassword")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(
                User,
                changePasswordDto.UserId,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); 

            if (!UserValidation.ValidateChangePasswordInput(changePasswordDto))
                return BadRequest("Please validate your input.");

            bool? result = await _userService.ChangePasswordAsync(changePasswordDto);

            if (result is null)
                return NotFound($"User with id: {changePasswordDto.UserId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while changing the password." });

            _loggerService.Log($"Changed user's password with id: {changePasswordDto.UserId}.", ip, userId);
            return Ok("Password is changed successfully.");
        }

        [HttpGet("{userId}", Name = "FindUserById")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<UserDto>> FindAsync(int userId)
        {
            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(
                User,
                userId,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            if (!UserValidation.ValidateUserId(userId))
                return BadRequest("Please validate your input.");

            UserDto user = await _userService.FindAsync(userId);

            if (user is null)
                return NotFound($"User with id: {userId} is not found.");

            return Ok(user);
        }

        [HttpPost("find", Name = "FindUserByCredentials")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<UserDto>> FindAsync(FindUserDto findUserDto)
        {
            if (!UserValidation.ValidateFindUserInput(findUserDto))
                return BadRequest("Please validate your input.");

            UserDto user = await _userService.FindAsync(findUserDto);

            if (user is null)
                return NotFound($"User is not found.");

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(
                User,
                user.UserId,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            return Ok(user);
        }

        [Authorize(Policy = "ManageUsers")]
        [HttpGet("isUsernameUsed/{username}", Name = "IsUsernameUsed")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool>> IsUsernameUsedAsync(string username)
        {
            if (!UserValidation.ValidateUsername(username))
                return BadRequest("Please validate your input.");

            return Ok(await _userService.IsUsernameUsedAsync(username));
        }

        [Authorize(Policy = "ManageUsers")]
        [HttpDelete("{userId}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool>> DeleteUserAsync(int userId)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!UserValidation.ValidateUserId(userId))
                return BadRequest("Please validate your input.");

            bool? result = await _userService.DeleteUserAsync(userId);

            if (result is null)
                return NotFound($"User with id: {userId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while deleting the user." });

            _loggerService.Log($"Deleted user with id: {userId}.", ip, userID);
            return Ok("User deleted successfully.");
        }

        [Authorize(Policy = "ManageUsers")]
        [HttpPatch("updateLastLoginDate/{userId}", Name = "UpdateUserLastLoginDate")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult> UpdateUserLastLoginDateAsync(int userId)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!UserValidation.ValidateUserId(userId))
                return BadRequest("Please validate your input.");

            bool? result = await _userService.UpdateUserLastLoginDateAsync(userId);

            if (result is null)
                return NotFound($"User with id: {userId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the user's last login date." });

            _loggerService.Log($"Updated user's last login date with id: {userId}.", ip, userID);
            return Ok("User's last login date is updated successfully.");
        }

        [Authorize(Policy = "ManageUsers")]
        [HttpGet("addAsCurrentUser/{userId}", Name = "AddAsCurrentUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<bool>> AddAsCurrentUserAsync(int userId)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            if (!UserValidation.ValidateUserId(userId))
                return BadRequest("Please validate your input.");

            bool? result = await _userService.AddAsCurrentUserAsync(userId);

            if (result is null)
                return NotFound($"User with id: {userId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the user as the current user." });

            _loggerService.Log($"Added user with id: {userId} as current user.", ip, userID);
            return Ok("User is added as the current user successfully.");
        }
    }
}
