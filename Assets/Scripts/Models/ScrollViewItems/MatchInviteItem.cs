using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models.ScrollViewItems
{
	public class MatchInviteItem
		: MonoBehaviour
	{
		private static bool isProcessing;

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
			if (isProcessing)
			{
				return;
			}

			isProcessing = true;
			StartCoroutine(ApiManager.ReplayCalls.GetReplay(
				Match.ReplayId.Value,
				onSuccess: replayData =>
				{
					GameManager.Instance.StartMultiplayerGame(Match, new Replay(replayData));
					isProcessing = false;
				},
				onFailure: error =>
				{
					GameManager.Instance.MenuManager.ShowErrorPopup();
					isProcessing = false;
				}));
		}
	}
}
