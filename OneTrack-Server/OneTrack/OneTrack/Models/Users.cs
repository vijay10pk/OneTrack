using System;
using System.ComponentModel.DataAnnotations;

namespace OneTrack.Models
{
	public class Users
	{
		[Key]
		public int UserID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string? Role { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}

