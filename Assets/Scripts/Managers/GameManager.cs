using Assets.Scripts.Controllers;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Managers
{
	public class GameManager
		: MonoBehaviour
	{
		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public static GameManager Instance { get; private set; }

		public User User { get; set; }

		public PlayerController Player { get; set; }

		public GuiManager GuiManager { get; set; }

		public bool Paused { get; internal set; }
	}
}
