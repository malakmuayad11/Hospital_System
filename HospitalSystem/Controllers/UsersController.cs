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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            List<UserDto> users = await _UserService.GetAllUsersAsync();

            if (users == null || users.Count == 0)
                return NotFound("Users are not found");

            return Ok(users);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetUsersCountAsync()
        {
            int UsersCount = await _UserService.GetUsersCountAsync();

            if(UsersCount == null|| UsersCount < 0)
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while counting users." });

            return Ok(UsersCount);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> AddNewUser(AddUserDto addUserDto)
        {
            if (!UserValidation.ValidateAddUserInput(addUserDto))
                return BadRequest("Please validate your input.");

            if(!await _UserService.AddUserAsync(addUserDto))
                return StatusCode(StatusCodes.Status500InternalServerError,
             new { message = "An error occurred while adding the new user." });

            return Ok();
            //return CreatedAtRoute("GetUserByID", new { userID = user.UserID }, user.userDTO);
        }

        // may fix it after creating a find user method
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>>UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            if (!UserValidation.ValidateUpdateUserInput(updateUserDto))
                return BadRequest("Please validate your input.");

            if(!await _UserService.UpdateUserAsync(updateUserDto))
                return NotFound($"User with id: {updateUserDto.UserId} is not found.");

            return Ok("User updated successfully.");
        }
    }
}
