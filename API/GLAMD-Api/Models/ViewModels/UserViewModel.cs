using System;

namespace GLAMD_Api.Models.ViewModels
{
	public class UserViewModel
	{
		public UserViewModel(User user)
		{
			Id = user.Id;
			Username = user.Username;
		}

		public Guid Id { get; private set; }

		public string Username { get; private set; }
	}
}