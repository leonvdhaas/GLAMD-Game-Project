using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Models;
using Assets.Scripts.Utilities;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using RNG = System.Random;

namespace Assets.Scripts.Managers
{
	public class GameManager
		: MonoBehaviour
	{
		private static readonly RNG rng = new RNG();
		private float unpausedTimeScale;

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

		public PlayerController Player { get; set; }

		public GuiManager GuiManager { get; set; }

		public User User { get; set; }

		private Game _currentGame;
		public Game CurrentGame
		{
			get
			{
				if (_currentGame != null)
				{
					return _currentGame;
				}

				return Game.Default;
			}
			private set
			{
				_currentGame = value;
			}
		}

		public void StartSingleplayerGame()
		{
			RandomUtilities.Seed = rng.Next(Int32.MinValue, Int32.MaxValue);
			StartGame(GameType.Singleplayer, null, null, null);
		}

		public void StartMultiplayerGame(Guid opponentId)
		{
			RandomUtilities.Seed = rng.Next(Int32.MinValue, Int32.MaxValue);
			StartGame(GameType.MultiplayerCreate, null, new Replay(), opponentId);
		}

		public void StartMultiplayerGame(Match match)
		{
			RandomUtilities.Seed = match.Seed;
			StartCoroutine(ApiManager.ReplayCalls.GetReplay(
				match.ReplayId.Value,
				onSuccess: replayData =>
				{
					StartGame(GameType.MultiplayerChallenge, match, new Replay(replayData), null);
				},
				onFailure: error =>
				{
					// TO-DO: Handle error.
				}));
		}

		private void StartGame(GameType gameType, Match match, Replay replay, Guid? opponentId)
		{
			CurrentGame = new Game
			{
				GameType = gameType,
				Match = match,
				Replay = replay,
				OpponentId = opponentId
			};

			// TO-DO: Watch advertisement before starting game.

			SceneManager.LoadScene("Main");
		}

		public void Logout()
		{
			User = null;
			CurrentGame = null;
		}

		public void Pause()
		{
			unpausedTimeScale = Time.timeScale;
			Time.timeScale = 0;
		}

		public void Unpause()
		{
			Time.timeScale = unpausedTimeScale;
		}
	}
}
