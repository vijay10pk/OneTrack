using System;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneTrack.Interfaces;
using OneTrack.Models;

namespace OneTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration _configuration;

        public UsersController(ILogger<UsersController> logger, IUsersRepository usersRepository)
        {
            _logger = logger;
            _usersRepository = usersRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Users>))]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _usersRepository.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{UserID}")]
        [ProducesResponseType(200, Type = typeof(Users))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserById(int UserID)
        {
            try
            {
                Users user = await _usersRepository.GetUserById(UserID);
                if (user == null)
                    return NotFound();
                else
                    return Ok(user);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register(Users userdata)
        {
            try
            {
                // Check if the user already exists
                var existingUser = await _usersRepository.GetUserByEmailAsync(userdata.Email);
                if (existingUser != null)
                    return BadRequest("Email is already registered");

                // Create a new user
                var user = new Users
                {
                    FirstName = userdata.FirstName,
                    LastName = userdata.LastName,
                    Email = userdata.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userdata.Password),
                    Role = "User"
                };

                await _usersRepository.CreateUserAsync(user);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] JsonPatchDocument<Users> patchUserData)
        {
            try
            {
                var user = await _usersRepository.GetUserById(id);
                if (user == null)
                    return NotFound();

                patchUserData.ApplyTo(user, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if(patchUserData.Operations.Any(o=> o.path.Equals("/password")))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }

                var updatedUser = await _usersRepository.UpdateUser(user);
                return Ok(updatedUser);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _usersRepository.DeleteUser(id);
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}

