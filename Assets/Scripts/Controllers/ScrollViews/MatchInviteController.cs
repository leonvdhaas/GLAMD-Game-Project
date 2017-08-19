using Assets.Scripts.Helpers;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Models.ScrollViewItems;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Controllers.ScrollViews
{
	public class MatchInviteController
		: MonoBehaviour
	{
		[SerializeField]
		private GameObject itemTemplate;

		[SerializeField]
		private GameObject itemHolder;

		private Match[] matches;

		private void OnEnable()
		{
			StartCoroutine(CoroutineHelper.Repeat(30, UpdateMatchInvites));
		}

		private void OnDisable()
		{
			StopAllCoroutines();
			RemoveItems();
		}

		private void UpdateMatchInvites()
		{
			StartCoroutine(ApiManager.UserCalls.UserMatchInvites(
				GameManager.Instance.User.Id,
				onSuccess: matches =>
				{
					this.matches = matches.OrderByDescending(x => x.CreatedOn).ToArray();
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

			foreach (var match in matches)
			{
				GameObject scrollViewItem = Instantiate(itemTemplate, itemHolder.transform, false);
				scrollViewItem.GetComponent<MatchInviteItem>().Match = match;
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
