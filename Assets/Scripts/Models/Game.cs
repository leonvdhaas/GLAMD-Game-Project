using Assets.Scripts.Enumerations;
using System;

namespace Assets.Scripts.Models
{
	public class Game
	{
		public GameType GameType { get; set; }

		public Replay Replay { get; set; }

		public Match Match { get; set; }

		public Guid? OpponentId { get; set; }

		public bool GameOver { get; set; }
	}
}
