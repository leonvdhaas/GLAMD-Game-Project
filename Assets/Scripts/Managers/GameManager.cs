using UnityEngine;

namespace Assets.Scripts.Managers
{
	public class GameManager
		: MonoBehaviour
	{
		// Use this for initialization
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

		public static GameManager Instance { get; private set; }

		public static TileManager TileManager
		{
			get
			{
				return TileManager.Instance;
			}
		}

		public static SoundManager SoundManager
		{
			get
			{
				return SoundManager.Instance;
			}
		}

		public static ApiManager ApiManager
		{
			get
			{
				return ApiManager.Instance;
			}
		}
	}
}
