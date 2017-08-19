using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models.ScrollViewItems
{
	public class FriendRequestItem
		: MonoBehaviour
	{
		[SerializeField]
		private Text lblFriendName;

		private User friendUser;

		private Friend _friend;
		public Friend Friend
		{
			get
			{
				return _friend;
			}
			set
			{
				_friend = value;

				if (Friend.UserId == GameManager.Instance.User.Id)
				{
					friendUser = new User
					{
						Id = Friend.InvitedId,
						Username = Friend.InvitedName
					};
				}
				else
				{
					friendUser = new User
					{
						Id = Friend.UserId,
						Username = Friend.UserName
					};
				}

				UpdateItem();
			}
		}

		private void UpdateItem()
		{
			lblFriendName.text = friendUser.Username;
		}

		public void AcceptFriendRequestButton()
		{
			StartCoroutine(ApiManager.FriendCalls.Accept(
				Friend.Id,
				onSuccess: friend =>
				{
					Destroy(gameObject);
				},
				onFailure: error =>
				{
					MenuManager.Instance.ShowErrorPopup();
				}));
		}

		public void RejectFriendRequestButton()
		{
			StartCoroutine(ApiManager.FriendCalls.Delete(
				Friend.Id,
				onSuccess: deletionMessage =>
				{
					Destroy(gameObject);
				},
				onFailure: error =>
				{
					MenuManager.Instance.ShowErrorPopup();
				}));
		}
	}
}
