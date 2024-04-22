using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using OneTrack.Interfaces;
using OneTrack.Models;
using OneTrack.Utils;

namespace OneTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
	{
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration _configuration;

        public LoginController(ILogger<UsersController> logger, IUsersRepository usersRepository, IConfiguration configuration)
        {
            _logger = logger;
            _usersRepository = usersRepository;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Find the user by email
            var user = await _usersRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Invalid email or password");

            // Verify the password
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                return Unauthorized("Invalid email or password");

            // Generate a JWT token
            var token = JwtTokenGeneratorUtil.GenerateJwtToken(_configuration,user);

            return Ok(new { token });
        }
    }

}

