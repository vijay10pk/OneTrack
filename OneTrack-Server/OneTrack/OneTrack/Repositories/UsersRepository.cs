using System;
using OneTrack.Data;
using OneTrack.Models;
using OneTrack.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OneTrack.Repositories
{
	public class UsersRepository : IUsersRepository
	{
		private AppDbContext _context;

		public UsersRepository(AppDbContext context)
		{
			_context = context;
		}

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() == 1;
        }

        public bool UserDataExists(int id)
        {
            return _context.Users.Any(users => users.UserID == id);
        }

        public async Task<Users> GetUserByEmailAsync(string email) => await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

        public async Task<List<Users>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> GetUserById(int UserID)
        {
            return await _context.Users.FindAsync(UserID);
        }

        public async Task<bool> CreateUserAsync(Users user)
        {
            _context.Users.Add(user);
            return await Save();
        }

        public async Task<Users> UpdateUser(Users user)
        {
            _context.Users.Update(user);
            await Save();
            return user;
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await Save();
            }
        }
    }
}

