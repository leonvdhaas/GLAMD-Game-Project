using System.ComponentModel.DataAnnotations;

namespace GLAMD_Api.Models
{
	public class Friend
		: Entity
	{
		[Required]
		public virtual User User { get; set; }

		[Required]
		public virtual User Invited { get; set; }

		public bool Accepted { get; set; }
	}
}