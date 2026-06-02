using HospitalSystem.API.Models;
using HospitalSystem.API.Validation;
using HospitalSystem.DTOs;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UsersController(IUserService UserService)
        {
            this._UserService = UserService;
        }

        [HttpGet(Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            List<UserDto> users = await _UserService.getAllUsersAsync();

            if (users == null || users.Count == 0)
                return NotFound("Users are not found");

            return Ok(users);
        }

        [HttpGet("count", Name = "GetUsersCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetUsersCountAsync()
        {
            int UsersCount = await _UserService.getUsersCountAsync();

            if (UsersCount == null || UsersCount < 0)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while counting users." });

            return Ok(UsersCount);
        }

        [HttpPost(Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> AddNewUserAsync(AddUserDto addUserDto)
        {
            if (!UserValidation.ValidateAddUserInput(addUserDto))
                return BadRequest("Please validate your input.");

            if (!await _UserService.addUserAsync(addUserDto))
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new user." });

            return Ok();
            //return CreatedAtRoute("GetUserByID", new { userID = user.UserID }, user.userDTO);
        }

        [HttpPut(Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            if (!UserValidation.ValidateUpdateUserInput(updateUserDto))
                return BadRequest("Please validate your input.");

            bool? result = await _UserService.updateUserAsync(updateUserDto);

            if (result == null)
                return NotFound($"User with id: {updateUserDto.UserId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the user." });

            return Ok("User updated successfully.");

        }

        [HttpPatch("changePassword", Name = "changePassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            if (!UserValidation.ValidateChangePasswordInput(changePasswordDto))
                return BadRequest("Please validate your input.");

            bool? result = await _UserService.changePasswordAsync(changePasswordDto);

            if (result == null)
                return NotFound($"User with id: {changePasswordDto.UserId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while changing the password." });

            return Ok("Password is changed successfully.");
        }

        [HttpGet("{UserId}", Name = "FindUserById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> FindAsync(int UserId)
        {
            if (!UserValidation.ValidateUserId(UserId))
                return BadRequest("Please validate your input.");

            UserDto user = await _UserService.findAsync(UserId);

            if (user == null)
                return NotFound($"User with id: {UserId} is not found.");

            return Ok(user);
        }

        [HttpPost("find", Name = "FindUserByCredentials")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> FindAsync(FindUserDto findUserDto)
        {
            if (!UserValidation.ValidateFindUserInput(findUserDto))
                return BadRequest("Please validate your input.");

            UserDto user = await _UserService.findAsync(findUserDto);

            if (user == null)
                return NotFound($"User with username: {findUserDto.Username} is not found.");

            return Ok(user);
        }

        [HttpGet("isUsernameUsed/{Username}", Name = "IsUsernameUsed")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> IsUsernameUsedAsync(string Username)
        {
            if (!UserValidation.ValidateUsername(Username))
                return BadRequest("Please validate your input.");

            return Ok(await _UserService.isUsernameUsedAsync(Username));
        }

        [HttpDelete("{UserId}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteUserAsync(int UserId)
        {
            if (!UserValidation.ValidateUserId(UserId))
                return BadRequest("Please validate your input.");

            bool? result = await _UserService.deleteUserAsync(UserId);

            if (result == null)
                return NotFound($"User with id: {UserId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while deleting the user." });

            return Ok("User deleted successfully.");
        }

        [HttpPatch("updateLastLoginDate/{UserId}", Name = "UpdateUserLastLoginDate")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateUserLastLoginDateAsync(int UserId)
        {
            if (!UserValidation.ValidateUserId(UserId))
                return BadRequest("Please validate your input.");

            bool? result = await _UserService.updateUserLastLoginDateAsync(UserId);

            if (result == null)
                return NotFound($"User with id: {UserId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while updating the user's last login date." });

            return Ok("User's last login date is updated successfully.");
        }

        [HttpGet("addAsCurrentUser/{UserId}", Name = "AddAsCurrentUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> AddAsCurrentUserAsync(int UserId)
        {
            if (!UserValidation.ValidateUserId(UserId))
                return BadRequest("Please validate your input.");

            bool? result = await _UserService.addAsCurrentUserAsync(UserId);

            if (result == null)
                return NotFound($"User with id: {UserId} is not found.");

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the user as the current user." });

            return Ok("User is added as the current user successfully.");
        }
    }
}
