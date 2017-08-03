using System;

namespace Assets.Scripts.Models
{
	public class Friend
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public Guid InvitedId { get; set; }

		public bool Accepted { get; set; }
	}
}
