using System;
using Microsoft.EntityFrameworkCore;
using OneTrack.Models;

namespace OneTrack.Data
{
	public class AppDbContext : DbContext
	{
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Users> Users { get; set; }
    }
}

