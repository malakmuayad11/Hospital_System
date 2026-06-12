using HospitalSystem.API.Validation;
using HospitalSystem.Infrastructure.DTOs.Users;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public UsersController(IUserService UserService, IAuthorizationService authorizationService)
        {
            this._userService = UserService;
            this._authorizationService = authorizationService;
        }

        [Authorize(Policy = "ManageUsers")]
        [HttpGet(Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        public async Task<ActionResult<UserDto>> AddNewUserAsync(AddUserDto addUserDto)
        {
            if (!UserValidation.ValidateAddUserInput(addUserDto))
                return BadRequest("Please validate your input.");

            int? userId = await _userService.AddUserAsync(addUserDto);

            if (userId is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new user." });

            return CreatedAtRoute("FindUserById", new { userId = userId }, addUserDto);
        }

        [HttpPut(Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            if (!UserValidation.ValidateUpdateUserInput(updateUserDto))
                return BadRequest("Please validate your input.");

            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(
                User,
                updateUserDto.UserId,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            bool? result = await _userService.UpdateUserAsync(updateUserDto);

            if (result is null)
                return NotFound($"User with id: {updateUserDto.UserId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the user." });

            return Ok("User is updated successfully.");
        }

        [HttpPatch("changePassword", Name = "changePassword")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(
                User,
                changePasswordDto.UserId,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            if (!UserValidation.ValidateChangePasswordInput(changePasswordDto))
                return BadRequest("Please validate your input.");

            bool? result = await _userService.ChangePasswordAsync(changePasswordDto);

            if (result is null)
                return NotFound($"User with id: {changePasswordDto.UserId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while changing the password." });

            return Ok("Password is changed successfully.");
        }

        [HttpGet("{userId}", Name = "FindUserById")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        public async Task<ActionResult<bool>> DeleteUserAsync(int userId)
        {
            if (!UserValidation.ValidateUserId(userId))
                return BadRequest("Please validate your input.");

            bool? result = await _userService.DeleteUserAsync(userId);

            if (result is null)
                return NotFound($"User with id: {userId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while deleting the user." });

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
        public async Task<ActionResult> UpdateUserLastLoginDateAsync(int userId)
        {
            if (!UserValidation.ValidateUserId(userId))
                return BadRequest("Please validate your input.");

            bool? result = await _userService.UpdateUserLastLoginDateAsync(userId);

            if (result is null)
                return NotFound($"User with id: {userId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the user's last login date." });

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
        public async Task<ActionResult<bool>> AddAsCurrentUserAsync(int userId)
        {
            if (!UserValidation.ValidateUserId(userId))
                return BadRequest("Please validate your input.");

            bool? result = await _userService.AddAsCurrentUserAsync(userId);

            if (result is null)
                return NotFound($"User with id: {userId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the user as the current user." });

            return Ok("User is added as the current user successfully.");
        }
    }
}
