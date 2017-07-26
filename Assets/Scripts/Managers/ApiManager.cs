using UnityEngine;

namespace Assets.Scripts.Managers
{
	public class ApiManager
		: MonoBehaviour
	{
		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);

			}
			else if (Instance != this)
			{
				Destroy(gameObject);
			}
		}

		public static ApiManager Instance { get; private set; }
	}
}
