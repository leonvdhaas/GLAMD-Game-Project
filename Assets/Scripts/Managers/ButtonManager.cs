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
		private Text lblErrorLogin;

		[SerializeField]
		private Text lblErrorUsername;

		[SerializeField]
		private Text lblErrorPassword;

		[SerializeField]
		private Text lblErrorConfirmPassword;

		public void StartGameBtn()
		{
			SceneManager.LoadScene("Main");
		}

		public void ExitGameBtn()
		{
			Application.Quit();
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

		public void LoginButton()
		{
			lblErrorLogin.enabled = false;

			var username = loginUsername.text;
			var password = loginPassword.text;

			if (false /* Try to login */)
			{
				// Login
			}
			else
			{
				SetError(lblErrorLogin, "Incorrecte login gegevens");
			}
		}

		public void RegisterButton()
		{
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
			else if (false /* Check if username exists */)
			{

			}

			if (password.Length < 6)
			{
				SetError(lblErrorPassword, "Minimaal 6 karakters");
			}
			
			if (password != confirmPassword)
			{
				SetError(lblErrorConfirmPassword, "Wachtwoorden komen niet overeen");
			}

			if (
				!lblErrorUsername.enabled &&
				!lblErrorPassword.enabled &&
				!lblErrorConfirmPassword.enabled)
			{
				// Register
			}
		}
	}
}
