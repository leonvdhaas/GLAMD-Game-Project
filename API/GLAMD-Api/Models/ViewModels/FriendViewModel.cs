using System;

namespace GLAMD_Api.Models.ViewModels
{
	public class FriendViewModel
	{
		public FriendViewModel(Friend friend)
		{
			Id = friend.Id;
			UserId = friend.User.Id;
			InvitedId = friend.Invited.Id;
			Accepted = friend.Accepted;
		}

		public Guid Id { get; private set; }

		public Guid UserId { get; private set; }

		public Guid InvitedId { get; private set; }

		public bool Accepted { get; private set; }
	}
}