using Assets.Scripts.Helpers;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Models.ScrollViewItems;
using UnityEngine;

namespace Assets.Scripts.Controllers.ScrollViews
{
	public class HighscoreController
		: MonoBehaviour
	{
		public const int AMOUNT_OF_HIGHSCORES = 10;

		[SerializeField]
		private GameObject itemTemplate;

		[SerializeField]
		private GameObject itemHolder;

		private RankedHighscore[] rankedHighscores;

		private void OnEnable()
		{
			StartCoroutine(CoroutineHelper.Repeat(30, UpdateHighscoreEntries));
		}

		private void OnDisable()
		{
			StopAllCoroutines();
			RemoveItems();
		}

		private void UpdateHighscoreEntries()
		{
			StartCoroutine(ApiManager.HighscoreCalls.HighscoreTop(
				AMOUNT_OF_HIGHSCORES,
				onSuccess: highscores =>
				{
					rankedHighscores = new RankedHighscore[highscores.Length];
					for (int i = 0; i < highscores.Length; i++)
					{
						rankedHighscores[i] = new RankedHighscore
						{
							Rank = i + 1,
							Highscore = highscores[i]
						};
					}

					UpdateScrollView();
				},
				onFailure: error =>
				{
					ButtonManager.Instance.ShowErrorPopup();
				}));
		}

		private void UpdateScrollView()
		{
			RemoveItems();

			foreach (var rankedHighscore in rankedHighscores)
			{
				GameObject scrollViewItem = Instantiate(itemTemplate, itemHolder.transform, false);
				scrollViewItem.GetComponent<HighscoreItem>().RankedHighscore = rankedHighscore;
			}
		}

		private void RemoveItems()
		{
			foreach (Transform item in itemHolder.transform)
			{
				Destroy(item.gameObject);
			}
		}
	}
}
