﻿using Assets.Scripts.Extensions;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models.ScrollViewItems
{
	public class ChallengeFriendItem
		: MonoBehaviour
	{
		private const int PANEL_DEPTH = 4;

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

		public void ChallengeFriendButton()
		{
			transform.GetParent(PANEL_DEPTH).gameObject.SetActive(false);
			GameManager.Instance.StartMultiplayerGame(friendUser.Id);
		}
	}
}