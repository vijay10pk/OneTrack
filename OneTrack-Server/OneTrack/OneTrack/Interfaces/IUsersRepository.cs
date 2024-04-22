using System;
using OneTrack.Models;

namespace OneTrack.Interfaces
{
	public interface IUsersRepository
    {
        Task<bool> Save();
        Task<Users> GetUserByEmailAsync(string Email);
        Task<List<Users>> GetAllUsers();
        Task<Users> GetUserById(int UserID);
        Task<bool> CreateUserAsync(Users user);
        Task<Users> UpdateUser(Users user);
        Task DeleteUser(int userId);
    }
}

