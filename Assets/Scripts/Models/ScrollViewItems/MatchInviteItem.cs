using Assets.Scripts.Extensions;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models.ScrollViewItems
{
	public class MatchInviteItem
		: MonoBehaviour
	{
		[SerializeField]
		private Text lblOpponentName;

		[SerializeField]
		private Text lblOpponentScore;

		private Match _match;
		public Match Match
		{
			get
			{
				return _match;
			}
			set
			{
				_match = value;
				UpdateItem();
			}
		}

		private void UpdateItem()
		{
			lblOpponentName.text = Match.CreatorName;
			lblOpponentScore.text = Match.CreatorScore.ToString();
		}

		public void MatchInviteButton()
		{
			GameManager.Instance.StartMultiplayerGame(Match);
		}
	}
}
