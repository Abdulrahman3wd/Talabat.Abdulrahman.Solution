using System.ComponentModel.DataAnnotations;

namespace TalabatAPIs.DTOs
{
	public class RegisterDto
	{
		[Required]
		public string DisplayName { get; set; } = null!;
		[Required]
		[EmailAddress]


		public string Email { get; set; } = null!;
		[Required]
		public string Phone { get; set; } = null!;
		[Required]
		[RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$")]
		public string Password { get; set; } = null!;
	}
}
