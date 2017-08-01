using GLAMD_Api.Models.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace GLAMD_Api.Models
{
	public class Match
		: Entity
	{
		public int Seed { get; set; }

		[Required]
		public Status Status { get; set; }

		public int CreatorScore { get; set; }

		public int? OpponentScore { get; set; }

		[Required]
		public virtual User Creator { get; set; }

		[Required]
		public virtual User Opponent { get; set; }		

		public virtual User Victor { get; set; }

		public virtual Replay Replay { get; set; }
	}
}