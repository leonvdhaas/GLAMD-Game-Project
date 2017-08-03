using Assets.Scripts.Enumerations;
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

		public virtual Guid CreatorId { get; set; }

		public virtual Guid OpponentId { get; set; }

		public virtual Guid? VictorId { get; set; }

		public virtual Guid? ReplayId { get; set; }
	}
}
