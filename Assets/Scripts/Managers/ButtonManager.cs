using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
	public class ButtonManager
		: MonoBehaviour
	{
		public void StartGameBtn()
		{
			SceneManager.LoadScene("Main");
		}

		public void ExitGameBtn()
		{
			Application.Quit();
		}
	}
}
