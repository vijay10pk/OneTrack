using System;
using Microsoft.IdentityModel.Tokens;
using OneTrack.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OneTrack.Controllers;
using OneTrack.Interfaces;

namespace OneTrack.Utils
{
    public class JwtTokenGeneratorUtil
    {
        public static string GenerateJwtToken(IConfiguration configuration, Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = string.IsNullOrEmpty(configuration["JwtSettings:Key"]) ? throw new ArgumentNullException("Key configuration is missing or empty") : Encoding.ASCII.GetBytes(configuration["JwtSettings:Key"]);
            string role = user.Role ?? "User";
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, role)
                }
                ),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

