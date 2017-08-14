using Assets.Scripts.Helpers;
using Assets.Scripts.Utilities;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets.Scripts.Managers
{
	public class ButtonManager
		: MonoBehaviour
	{
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

		private bool waitingForLogin;
		private bool waitingForRegister;

		public void StartGameBtn()
		{
			SceneManager.LoadScene("Main");
		}

		public void LoginButton()
		{
			if (waitingForLogin)
			{
				return;
			}

			waitingForLogin = true;
			lblErrorLogin.enabled = false;

			var username = loginUsername.text;
			var password = loginPassword.text;

			if (username.Length == 0 || password.Length == 0)
			{
				SetError(lblErrorLogin, "Vul uw login gegevens in");
				waitingForLogin = true;
				return;
			}

			StartCoroutine(ApiManager.UserCalls.LoginUser(
				username,
				Hasher.Hash(password),
				onSuccess: user =>
				{
					GameManager.Instance.User = user;

					loginPanel.SetActive(false);
					homePanel.SetActive(true);

					waitingForLogin = false;
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

					waitingForLogin = false;
				}));
		}

		public void RegisterButton()
		{
			if (waitingForRegister)
			{
				return;
			}

			waitingForRegister = true;
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
				waitingForRegister = false;
				return;
			}

			StartCoroutine(ApiManager.UserCalls.UserExists(
				username,
				onSuccess: exists =>
				{
					if (exists)
					{
						SetError(lblErrorUsername, "Gebruikersnaam is al in gebruik");
						waitingForRegister = false;
					}
					else
					{
						StartCoroutine(ApiManager.UserCalls.CreateUser(
							username,
							Hasher.Hash(password),
							onSuccess: user =>
							{
								GameManager.Instance.User = user;

								registerPanel.SetActive(false);
								homePanel.SetActive(true);
								waitingForRegister = false;
							},
							onFailure: error =>
							{
								SetError(lblErrorConfirmPassword, "Er is iets fout gegaan");
								waitingForRegister = false;
							}));
					}
				},
				onFailure: error =>
				{
					SetError(lblErrorConfirmPassword, "Er is iets fout gegaan");
					waitingForRegister = false;
				}));
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
