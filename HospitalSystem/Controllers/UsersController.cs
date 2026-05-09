using HospitalSystem.API.Models;
using HospitalSystem.API.Validation;
using HospitalSystem.DTOs;
using HospitalSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    //[Route("api/[controller]")]
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
            List<UserDto> users = await _UserService.GetAllUsersAsync();

            if (users == null || users.Count == 0)
                return NotFound("Users are not found");

            return Ok(users);
        }

        [HttpGet("count", Name = "GetUsersCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetUsersCountAsync()
        {
            int UsersCount = await _UserService.GetUsersCountAsync();

            if (UsersCount == null || UsersCount < 0)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while counting users." });

            return Ok(UsersCount);
        }

        [HttpPost(Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> AddNewUser(AddUserDto addUserDto)
        {
            if (!UserValidation.ValidateAddUserInput(addUserDto))
                return BadRequest("Please validate your input.");

            if (!await _UserService.AddUserAsync(addUserDto))
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new user." });

            return Ok();
            //return CreatedAtRoute("GetUserByID", new { userID = user.UserID }, user.userDTO);
        }

        // may fix it after creating a find user method
        [HttpPut(Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            if (!UserValidation.ValidateUpdateUserInput(updateUserDto))
                return BadRequest("Please validate your input.");

            if (!await _UserService.UpdateUserAsync(updateUserDto))
                return NotFound($"User with id: {updateUserDto.UserId} is not found.");

            return Ok("User updated successfully.");
        }

        [HttpPatch("changePassword", Name = "changePassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            if (!UserValidation.ValidateChangePasswordInput(changePasswordDto))
                return BadRequest("Please validate your input.");

            if (!await _UserService.ChangePasswordAsync(changePasswordDto))
                return NotFound($"User with id: {changePasswordDto.UserId} is not found.");

            return Ok("Password changed successfully.");
        }

        [HttpGet("{UserId}", Name = "GetUserByID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> Find(int UserId)
        {
            if(!UserValidation.ValidateUserId(UserId))
                return BadRequest("Please validate your input.");

            UserDto user = await _UserService.Find(UserId);

            if (user == null)
                return NotFound($"User with id: {UserId} is not found.");

            return Ok(user);
        }
    }
}
