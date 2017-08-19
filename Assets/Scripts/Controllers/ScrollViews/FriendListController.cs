using Assets.Scripts.Helpers;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Models.ScrollViewItems;
using UnityEngine;

namespace Assets.Scripts.Controllers.ScrollViews
{
	class FriendListController
		: MonoBehaviour
	{
		[SerializeField]
		private GameObject itemTemplate;

		[SerializeField]
		private GameObject itemHolder;

		private Friend[] friends;

		private void OnEnable()
		{
			StartCoroutine(CoroutineHelper.Repeat(30, UpdateFriends));
		}

		private void OnDisable()
		{
			StopAllCoroutines();
			RemoveItems();
		}

		private void UpdateFriends()
		{
			StartCoroutine(ApiManager.UserCalls.UserFriends(
				GameManager.Instance.User.Id,
				onSuccess: friends =>
				{
					this.friends = friends;
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

			foreach (var friend in friends)
			{
				GameObject scrollViewItem = Instantiate(itemTemplate, itemHolder.transform, false);
				scrollViewItem.GetComponent<FriendListItem>().Friend = friend;
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
