using Assets.Scripts.Enumerations;
using Assets.Scripts.Managers;
using System;

namespace Assets.Scripts.Models
{
	public class Match
	{
		public Guid Id { get; set; }

		public int Seed { get; set; }

		public MatchStatus Status { get; set; }

		public int CreatorScore { get; set; }

		public int? OpponentScore { get; set; }

		public Guid CreatorId { get; set; }

		public Guid OpponentId { get; set; }

		public Guid? VictorId { get; set; }

		public Guid? ReplayId { get; set; }

		public MatchWinner MatchWinner
		{
			get
			{
				if (!VictorId.HasValue)
				{
					return MatchWinner.Draw;
				}
				else if (VictorId.Value == GameManager.Instance.User.Id)
				{
					return MatchWinner.User;
				}
				else
				{
					return MatchWinner.Opponent;
				}
			}
		}
	}
}
