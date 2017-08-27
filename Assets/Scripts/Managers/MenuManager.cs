using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Helpers;
using Assets.Scripts.Utilities;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.Managers
{
	public class MenuManager
		: MonoBehaviour
	{
		[Header("Panels")]
		[SerializeField]
		private GameObject homePanel;
		[SerializeField]
		private GameObject registerPanel;
		[SerializeField]
		private GameObject loginPanel;
		[SerializeField]
		private GameObject errorPopupPanel;
		[SerializeField]
		private GameObject manualPanel;
		[SerializeField]
		private GameObject loadingPanel;
		[SerializeField]
		private GameObject controlsLoadingPanel;
		[SerializeField]
		private GameObject powerupsLoadingPanel;

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
		[SerializeField]
		private InputField addFriendUsername;

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
		[SerializeField]
		private Text lblAddFriendResult;
		[SerializeField]
		private Text lblCategory;

		[Header("Settings")]
		[SerializeField]
		private Slider soundEffectSlider;
		[SerializeField]
		private Slider musicSlider;
		[SerializeField]
		private Toggle instructionsToggle;

		[Header("Colors")]
		[SerializeField]
		private Color errorColor;
		[SerializeField]
		private Color successColor;

		private bool isProcessingButton;
		private Coroutine sfxSliderDrag;
		private ManualController manualController;
		private bool playSample;

		private void OnEnable()
		{
			SceneManager.sceneLoaded += SceneManager_SceneLoaded;

			GameManager.Instance.MenuManager = this;
			manualController = manualPanel.GetComponent<ManualController>();
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
		}

		private void SceneManager_SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			UpdateSettings();

			if (scene.name == "MainStartMenu" && GameManager.Instance.User != null)
			{
				SetUsername(GameManager.Instance.User.Username);
				homePanel.SetActive(true);
			}
			else
			{
				loginPanel.SetActive(true);
			}
		}

		public void ShowLoadingScreen()
		{
			loadingPanel.SetActive(true);
		}

		public void ShowLoadingScreen(int part)
		{
			switch (part)
			{
				case 0:
					controlsLoadingPanel.SetActive(true);
					break;
				case 1:
					powerupsLoadingPanel.SetActive(true);
					controlsLoadingPanel.SetActive(false);
					break;
				default:
					throw new InvalidOperationException("Unknown loading screen part provided.");
			}
		}

		private void UpdateSettings()
		{
			playSample = false;

			soundEffectSlider.value = SoundManager.Instance.SoundEffectVolume;;
			musicSlider.value = SoundManager.Instance.MusicVolume;
			instructionsToggle.isOn = !GameManager.Instance.SkipInstructions;

			StartCoroutine(CoroutineHelper.Delay(0.5f, () => playSample = true));
		}

		public void ShowErrorPopup()
		{
			errorPopupPanel.SetActive(true);
		}

		public void StartSingleplayerButton()
		{
			homePanel.SetActive(false);
			GameManager.Instance.StartSingleplayerGame();
		}

		public void SendFriendRequestButton()
		{
			if (isProcessingButton)
			{
				return;
			}

			lblAddFriendResult.text = String.Empty;

			string friendName = addFriendUsername.text;
			if (friendName.Length == 0)
			{
				lblAddFriendResult.text = "Vul een naam in";
				lblAddFriendResult.color = errorColor;
			}
			else if (friendName == GameManager.Instance.User.Username)
			{
				lblAddFriendResult.text = "Je kan jezelf niet toevoegen";
				lblAddFriendResult.color = errorColor;
			}

			if (lblAddFriendResult.text != String.Empty)
			{
				return;
			}

			isProcessingButton = true;

			StartCoroutine(ApiManager.UserCalls.UserExists(
				friendName,
				onSuccess: friendId =>
				{
					if (!friendId.HasValue)
					{
						lblAddFriendResult.text = "Kan vriend niet vinden";
						lblAddFriendResult.color = errorColor;
						isProcessingButton = false;
						return;
					}

					StartCoroutine(ApiManager.FriendCalls.Invite(
						GameManager.Instance.User.Id,
						friendId.Value,
						onSuccess: friendRequest =>
						{
							lblAddFriendResult.text = "Vriendschapsverzoek verstuurd";
							lblAddFriendResult.color = successColor;
							isProcessingButton = false;
						},
						onFailure: error =>
						{
							lblAddFriendResult.text = "Er is iets fout gegaan";
							lblAddFriendResult.color = errorColor;
							isProcessingButton = false;
						}));
				},
				onFailure: error =>
				{
					lblAddFriendResult.text = "Er is iets fout gegaan";
					lblAddFriendResult.color = errorColor;
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
				SetLoginError(lblErrorLogin, "Vul uw login gegevens in");
				isProcessingButton = false;
				return;
			}

			StartCoroutine(ApiManager.UserCalls.LoginUser(
				username,
				Hasher.Hash(password),
				onSuccess: user =>
				{
					GameManager.Instance.User = user;

					SetUsername(username);
					ClearInputFieldsAndErrors();
					loginPanel.SetActive(false);
					homePanel.SetActive(true);
					isProcessingButton = false;
				},
				onFailure: error =>
				{
					if (error.Message.Contains("Invalid"))
					{
						SetLoginError(lblErrorLogin, "Incorrecte login gegevens");
					}
					else
					{
						SetLoginError(lblErrorLogin, "Er is iets fout gegaan");
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
				SetLoginError(lblErrorUsername, "Minimaal 4 karakters");
			}
			else if (username.Length > 17)
			{
				SetLoginError(lblErrorUsername, "Maximaal 17 karakters");
			}
			else if (!ValidChars(username))
			{
				SetLoginError(lblErrorUsername, "Alleen A-Z, a-z en 0-9");
			}

			if (password.Length < 6)
			{
				SetLoginError(lblErrorPassword, "Minimaal 6 karakters");
			}
			
			if (password != confirmPassword)
			{
				SetLoginError(lblErrorConfirmPassword, "Wachtwoorden komen niet overeen");
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
						SetLoginError(lblErrorUsername, "Gebruikersnaam is al in gebruik");
						isProcessingButton = false;
						return;
					}

					StartCoroutine(ApiManager.UserCalls.CreateUser(
						username,
						Hasher.Hash(password),
						onSuccess: user =>
						{
							GameManager.Instance.User = user;

							SetUsername(username);
							ClearInputFieldsAndErrors();
							registerPanel.SetActive(false);
							homePanel.SetActive(true);
							isProcessingButton = false;
						},
						onFailure: error =>
						{
							SetLoginError(lblErrorConfirmPassword, "Er is iets fout gegaan");
							isProcessingButton = false;
						}));
				},
				onFailure: error =>
				{
					SetLoginError(lblErrorConfirmPassword, "Er is iets fout gegaan");
					isProcessingButton = false;
				}));
		}

		private void SetUsername(string username)
		{
			lblLoggedInAs.text = String.Format("Gebruiker: {0}", username);
		}

		public void ClearInputFieldsAndErrors()
		{
			registerUsername.text = String.Empty;
			registerPassword.text = String.Empty;
			registerConfirmPassword.text = String.Empty;
			loginUsername.text = String.Empty;
			loginPassword.text = String.Empty;

			lblErrorLogin.text = String.Empty;
			lblErrorUsername.text = String.Empty;
			lblErrorPassword.text = String.Empty;
			lblErrorConfirmPassword.text = String.Empty;
		}

		public void LogoutButton()
		{
			GameManager.Instance.Logout();
			lblLoggedInAs.text = String.Empty;
			ClearInputFieldsAndErrors();
			homePanel.SetActive(false);
			loginPanel.SetActive(true);
		}

		private bool ValidChars(string input)
		{
			const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return !input.Any(x => !validChars.Contains(x));
		}

		private void SetLoginError(Text errorLabel, string value)
		{
			errorLabel.text = value;
			errorLabel.enabled = true;
		}

		public void PreviousManualEntryButton()
		{
			manualController.PreviousManualEntry();
		}

		public void NextManualEntryButton()
		{
			manualController.NextManualEntry();
		}

		public void LoadManualEntries(string category)
		{
			manualController.LoadedPageEntries = manualController.PageEntries.Where(p => p.Category == category).ToArray();
			manualController.ResetPage();
			manualController.UpdateActiveManualEntry();
			lblCategory.text = category;
			manualPanel.SetActive(true);
		}

		public void SetSoundEffectVolume(float volume)
		{
			if (sfxSliderDrag != null)
			{
				StopCoroutine(sfxSliderDrag);
			}

			SoundManager.Instance.SoundEffectVolume = volume;

			if (playSample)
			{
				sfxSliderDrag = StartCoroutine(CoroutineHelper.Delay(0.1f, () => SoundManager.Instance.PlaySoundEffect(Sound.Diamond)));
			}
		}

		public void SetMusicVolume(float volume)
		{
			SoundManager.Instance.MusicVolume = volume;
		}

		public void SetShowInstructions(bool enabled)
		{
			GameManager.Instance.SkipInstructions = !enabled;
		}
	}
}
