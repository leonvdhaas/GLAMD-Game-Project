using System.ComponentModel.DataAnnotations;

namespace GLAMD_Api.Models
{
	public class User
		: Entity
	{
		public const int USERNAME_MINIMUM_LENGTH = 4;
		public const int USERNAME_MAXIMUM_LENGTH = 16;
		public const int PASSWORD_LENGTH = 128;

		[Required(AllowEmptyStrings = false)]
		[StringLength(USERNAME_MAXIMUM_LENGTH, MinimumLength = USERNAME_MINIMUM_LENGTH)]
		public string Username { get; set; }
		
		[Required(AllowEmptyStrings = false)]
		[StringLength(PASSWORD_LENGTH, MinimumLength = PASSWORD_LENGTH)]
		public string Password { get; set; }
	}
}