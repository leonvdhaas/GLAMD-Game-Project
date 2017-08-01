using GLAMD_Api.Models.Enumerations;
using System;

namespace GLAMD_Api.Models.ViewModels
{
	public class MatchViewModel
	{
		public MatchViewModel(Match match)
		{
			Id = match.Id;
			Seed = match.Seed;
			CreatorScore = match.CreatorScore;
			OpponentScore = match.OpponentScore;
			CreatorId = match.Creator.Id;
			OpponentId = match.Opponent.Id;
			VictorId = match.Victor?.Id;
			ReplayId = match.Replay?.Id;
		}

		public Guid Id { get; private set; }

		public int Seed { get; private set; }

		public Status Status { get; private set; }

		public int CreatorScore { get; private set; }

		public int? OpponentScore { get; private set; }

		public virtual Guid CreatorId { get; private set; }

		public virtual Guid OpponentId { get; private set; }

		public virtual Guid? VictorId { get; private set; }

		public virtual Guid? ReplayId { get; private set; }
	}
}