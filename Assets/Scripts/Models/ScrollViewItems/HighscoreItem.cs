using Assets.Scripts.Controllers.ScrollViews;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models.ScrollViewItems
{
	public class HighscoreItem
		: MonoBehaviour
	{
		[SerializeField]
		private Text lblRankAndName;

		[SerializeField]
		private Text lblScore;

		private RankedHighscore _rankedHighscore;
		public RankedHighscore RankedHighscore
		{
			get
			{
				return _rankedHighscore;
			}
			set
			{
				_rankedHighscore = value;
				UpdateItem();
			}
		}

		private void UpdateItem()
		{
			lblRankAndName.text = String.Format(
				"{0} {1}",
				String.Format("{0}.", RankedHighscore.Rank.ToString()).PadRight(HighscoreController.AMOUNT_OF_HIGHSCORES.ToString().Length + 1, ' '),
				RankedHighscore.Highscore.Username);
			lblScore.text = RankedHighscore.Highscore.Score.ToString();
		}
	}
}
