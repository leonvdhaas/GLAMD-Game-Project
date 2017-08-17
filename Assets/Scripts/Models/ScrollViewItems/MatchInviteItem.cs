using Assets.Scripts.Extensions;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models.ScrollViewItems
{
	public class MatchInviteItem
		: MonoBehaviour
	{
		private const int PANEL_DEPTH = 4;

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
			transform.GetParent(PANEL_DEPTH).gameObject.SetActive(false);
			GameManager.Instance.StartMultiplayerGame(Match);
		}
	}
}
