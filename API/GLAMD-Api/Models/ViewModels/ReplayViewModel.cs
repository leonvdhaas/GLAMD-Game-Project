using System;

namespace GLAMD_Api.Models.ViewModels
{
	public class ReplayViewModel
	{
		public ReplayViewModel(Replay replay)
		{
			Id = replay.Id;
		}

		public Guid Id { get; private set; }
	}
}