using UnityEngine;

namespace Assets.Scripts.Managers
{
	public class SoundManager
		: MonoBehaviour
	{
		// Use this for initialization
		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;

			}
			else if (Instance != this)
			{
				Destroy(gameObject);
			}

			DontDestroyOnLoad(gameObject);
		}

		public static SoundManager Instance { get; private set; }
	}
}
