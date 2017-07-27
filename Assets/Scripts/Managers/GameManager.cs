using Assets.Scripts.Controllers;
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

		public PlayerController Player { get; set; }

	}
}
