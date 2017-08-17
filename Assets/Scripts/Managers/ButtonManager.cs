using Assets.Scripts.Models;
using Assets.Scripts.Utilities;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.Managers
{
	public class ButtonManager
		: MonoBehaviour
	{
		[SerializeField]
		private bool SkipLogin;

		[Header("Panels")]
		[SerializeField]
		private GameObject homePanel;
		[SerializeField]
		private GameObject registerPanel;
		[SerializeField]
		private GameObject loginPanel;

		[Header("Input Fields")]
		[SerializeField]
		private InputField loginUsername;
		[SerializeField]
		private InputField loginPassword;
		[SerializeField]
		private InputField registerUsername;
		[SerializeField]
		private InputField registerPassword;
		[SerializeField]
		private InputField registerConfirmPassword;

		[Header("Text Fields")]
		[SerializeField]
		private Text lblErrorLogin;
		[SerializeField]
		private Text lblErrorUsername;
		[SerializeField]
		private Text lblErrorPassword;
		[SerializeField]
		private Text lblErrorConfirmPassword;
		[SerializeField]
		private Text lblLoggedInAs;

		private bool isProcessingButton;

		private void OnEnable()
		{
			SceneManager.sceneLoaded += SceneManager_SceneLoaded;
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
		}

		private void SceneManager_SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			if (scene.name == "MainStartMenu")
			{
				if (GameManager.Instance.User != null || SkipLogin)
				{
					homePanel.SetActive(true);
				}
				else
				{
					loginPanel.SetActive(true);
				}
			}
		}

		public void MultiplayerButton()
		{
			
		}

		public void StartSingleplayerButton()
		{
			GameManager.Instance.StartSingleplayerGame();
		}

		public void CreateMultiplayerChallengeButton()
		{
			// TO-DO: Get opponent id.
			Guid opponentId = default(Guid);
			GameManager.Instance.StartMultiplayerGame(opponentId);
		}

		public void AcceptMultiplayerChallengeButton()
		{
			// TO-DO: Get match.
			Match match = null;
			GameManager.Instance.StartMultiplayerGame(match);
		}

		public void AcceptFriendRequestButton()
		{
			if (isProcessingButton)
			{
				return;
			}

			isProcessingButton = true;

			// TO-DO: Get friend request id.
			var friendRequestId = Guid.NewGuid();

			StartCoroutine(ApiManager.FriendCalls.Accept(
				friendRequestId,
				onSuccess: friendRequest =>
				{
					// TO-DO: Display added friend.
					isProcessingButton = false;
				},
				onFailure: error =>
				{
					// TO-DO: Handle error.
					isProcessingButton = false;
				}));
		}

		public void SendFriendRequestButton()
		{
			if (isProcessingButton)
			{
				return;
			}

			isProcessingButton = true;

			// TO-DO: Get friend's name.
			var friendName = "";

			if (friendName.Length == 0)
			{
				// TO-DO: Display empty input field error.
				isProcessingButton = false;
				return;
			}

			StartCoroutine(ApiManager.UserCalls.UserExists(
				friendName,
				onSuccess: friendId =>
				{
					if (!friendId.HasValue)
					{
						// TO-DO: Display can't find friend error.
						isProcessingButton = false;
						return;
					}

					StartCoroutine(ApiManager.FriendCalls.Invite(
						GameManager.Instance.User.Id,
						friendId.Value,
						onSuccess: friendRequest =>
						{
							// TO-DO: Display successfully send friend request.
							isProcessingButton = false;
						},
						onFailure: error =>
						{
							// TO-DO: Handle error.
							isProcessingButton = false;
						}));
				},
				onFailure: error =>
				{
					// TO-DO: Handle error.
					isProcessingButton = false;
				}));
		}

		public void LoginButton()
		{
			if (isProcessingButton)
			{
				return;
			}

			isProcessingButton = true;
			lblErrorLogin.enabled = false;

			var username = loginUsername.text;
			var password = loginPassword.text;

			if (username.Length == 0 || password.Length == 0)
			{
				SetError(lblErrorLogin, "Vul uw login gegevens in");
				isProcessingButton = false;
				return;
			}

			StartCoroutine(ApiManager.UserCalls.LoginUser(
				username,
				Hasher.Hash(password),
				onSuccess: user =>
				{
					GameManager.Instance.User = user;

					lblLoggedInAs.text = String.Format("Ingelogd als: {0}", username);
					loginPanel.SetActive(false);
					homePanel.SetActive(true);

					isProcessingButton = false;
				},
				onFailure: error =>
				{
					if (error.Message.Contains("Invalid"))
					{
						SetError(lblErrorLogin, "Incorrecte login gegevens");
					}
					else
					{
						SetError(lblErrorLogin, "Er is iets fout gegaan");
					}

					isProcessingButton = false;
				}));
		}

		public void RegisterButton()
		{
			if (isProcessingButton)
			{
				return;
			}

			isProcessingButton = true;
			lblErrorUsername.enabled = lblErrorPassword.enabled = lblErrorConfirmPassword.enabled = false;

			var username = registerUsername.text;
			var password = registerPassword.text;
			var confirmPassword = registerConfirmPassword.text;

			if (username.Length < 4)
			{
				SetError(lblErrorUsername, "Minimaal 4 karakters");
			}
			else if (username.Length > 21)
			{
				SetError(lblErrorUsername, "Maximaal 21 karakters");
			}
			else if (!ValidChars(username))
			{
				SetError(lblErrorUsername, "Alleen A-Z, a-z en 0-9");
			}

			if (password.Length < 6)
			{
				SetError(lblErrorPassword, "Minimaal 6 karakters");
			}
			
			if (password != confirmPassword)
			{
				SetError(lblErrorConfirmPassword, "Wachtwoorden komen niet overeen");
			}

			if (lblErrorUsername.enabled ||
				lblErrorPassword.enabled ||
				lblErrorConfirmPassword.enabled)
			{
				isProcessingButton = false;
				return;
			}

			StartCoroutine(ApiManager.UserCalls.UserExists(
				username,
				onSuccess: userId =>
				{
					if (userId.HasValue)
					{
						SetError(lblErrorUsername, "Gebruikersnaam is al in gebruik");
						isProcessingButton = false;
						return;
					}

					StartCoroutine(ApiManager.UserCalls.CreateUser(
						username,
						Hasher.Hash(password),
						onSuccess: user =>
						{
							GameManager.Instance.User = user;

							registerPanel.SetActive(false);
							homePanel.SetActive(true);
							isProcessingButton = false;
						},
						onFailure: error =>
						{
							SetError(lblErrorConfirmPassword, "Er is iets fout gegaan");
							isProcessingButton = false;
						}));
				},
				onFailure: error =>
				{
					SetError(lblErrorConfirmPassword, "Er is iets fout gegaan");
					isProcessingButton = false;
				}));
		}

		public void LogoutButton()
		{
			GameManager.Instance.Logout();
			lblLoggedInAs.text = "";
			homePanel.SetActive(false);
			loginPanel.SetActive(true);
		}

		private bool ValidChars(string input)
		{
			const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return !input.Any(x => !validChars.Contains(x));
		}

		private void SetError(Text errorLabel, string value)
		{
			errorLabel.text = value;
			errorLabel.enabled = true;
		}
	}
}
