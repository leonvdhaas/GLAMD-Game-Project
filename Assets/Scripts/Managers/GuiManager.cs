﻿using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using PlayfulSystems.ProgressBar;
using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Models;
using UnityEngine.SceneManagement;
using Assets.Scripts.Helpers;
using Assets.Scripts.Utilities;

namespace Assets.Scripts.Managers
{
	public class GuiManager
		: MonoBehaviour
	{
		private const float METER_DELTA = 0.1f;

		[SerializeField]
		private Text coinsText;
		[SerializeField]
		private Text pointsText;
		[SerializeField]
		private Text pauseText;
		[SerializeField]
		private Image leftHeart;
		[SerializeField]
		private Image middleHeart;
		[SerializeField]
		private Image rightHeart;
		[SerializeField]
		private GameObject coinDoublerBar;
		[SerializeField]
		private GameObject inhalerBar;
		[SerializeField]
		private GameObject slowmotionBar;
		[SerializeField]
		private GameObject inhalerButton;
		[SerializeField]
		private Color errorColor;
		[SerializeField]
		private Color regularColor;

		[Header("End Screen Panels")]
		[SerializeField]
		private GameObject endScreenPanel;
		[SerializeField]
		private GameObject singleplayerPanel;
		[SerializeField]
		private GameObject multiplayerCreatePanel;
		[SerializeField]
		private GameObject multiplayerChallengePanel;
		[SerializeField]
		private GameObject endScreenExitButton;
		[SerializeField]
		private GameObject factPanel;

		[Header("SP End Screen")]
		[SerializeField]
		private Text spPoints;
		[SerializeField]
		private Text spCoins;
		[SerializeField]
		private Text spTotal;
		[SerializeField]
		private Text spHighscore;
		[SerializeField]
		private Text spHighscoreResult;

		[Header("MP Create End Screen")]
		[SerializeField]
		private Text mpCreatePoints;
		[SerializeField]
		private Text mpCreateCoins;
		[SerializeField]
		private Text mpCreateTotal;
		[SerializeField]
		private Text mpCreateInfo;

		[Header("MP Challenge End Screen")]
		[SerializeField]
		private Color winColor;
		[SerializeField]
		private Color drawColor;
		[SerializeField]
		private Color lossColor;
		[SerializeField]
		private Text mpChallengeMatchResult;
		[SerializeField]
		private Text mpErrorMessage;
		[SerializeField]
		private Text mpChallengePoints;
		[SerializeField]
		private Text mpChallengeCoins;
		[SerializeField]
		private Text mpChallengeTotal;
		[SerializeField]
		private Text mpChallengeFriendTotal;

		[Header("Facts screen")]
		[SerializeField]
		private Text fact;
		[SerializeField]
		private string[] facts;

		private float targetCoinDoublerMeter;
		private float targetInhalerMeter;
		private float targetSlowmotionMeter;

		private ProgressBarPro coinDoubler;
		private ProgressBarPro inhaler;
		private ProgressBarPro slowmotion;

		private void Start()
		{
			GameManager.Instance.GuiManager = this;

			coinDoubler = coinDoublerBar.GetComponent<ProgressBarPro>();
			inhaler = inhalerBar.GetComponent<ProgressBarPro>();
			slowmotion = slowmotionBar.GetComponent<ProgressBarPro>();

			inhalerBar.GetComponentInChildren<BarViewSizeImageFill>().SetNumSteps(PickupController.MAX_NUMBER_OF_INHALERS);
			inhalerBar.GetComponentInChildren<BarViewValueText>().SetMaxValue(PickupController.MAX_NUMBER_OF_INHALERS);
		}

		public void UpdateCoinDoublerMeter(float percentage)
		{
			targetCoinDoublerMeter = percentage;
		}

		public void UpdateInhalerMeter(float percentage)
		{
			targetInhalerMeter = percentage;

			if (Mathf.Approximately(percentage, 1))
			{
				inhalerButton.SetActive(true);
				inhalerButton.GetComponent<Button>().interactable = true;
			}
			else if (Mathf.Approximately(percentage, 0))
			{
				inhalerButton.SetActive(false);
			}
		}

		public void UpdateSlowmotionMeter(float percentage)
		{
			targetSlowmotionMeter = percentage;
		}

		private void FixedUpdate()
		{
			UpdateProgressBar(inhaler, targetInhalerMeter);
			UpdateProgressBar(coinDoubler, targetCoinDoublerMeter);
			UpdateProgressBar(slowmotion, targetSlowmotionMeter);
		}

		private void UpdateProgressBar(ProgressBarPro progressBar, float target)
		{
			if (!Mathf.Approximately(progressBar.Value, target))
			{
				progressBar.Value = Mathf.MoveTowards(progressBar.Value, target, METER_DELTA);
			}
		}

		public void UpdatePoints(int points)
		{
			pointsText.text = points.ToString().PadLeft(6, '0');
		}

		public void UpdateLives(int lives, bool blink)
		{
			switch (lives)
			{
				case 0:
					FlashDisableHeart(leftHeart, blink ? 5 : 0);
					middleHeart.enabled = rightHeart.enabled = false;
					break;
				case 1:
					leftHeart.enabled = true;
					FlashDisableHeart(middleHeart, blink ? 5 : 0);
					rightHeart.enabled = false;
					break;
				case 2:
					leftHeart.enabled = middleHeart.enabled = true;
					FlashDisableHeart(rightHeart, blink ? 5 : 0);
					break;
				case 3:
					leftHeart.enabled = middleHeart.enabled = rightHeart.enabled = true;
					break;
				default:
					throw new InvalidOperationException("Invalid amount of lives.");
			}
		}

		private void FlashDisableHeart(Image heart, int amount)
		{
			StartCoroutine(CoroutineHelper.RepeatFor(
						0.25f,
						amount,
						() => heart.enabled = !heart.enabled,
						() => heart.enabled = false));
		}

		public void UpdateCoins(int coins)
		{
			coinsText.text = String.Format("× {0}", coins.ToString().PadLeft(3, '0'));
		}

		public void DisplaySingleplayerEndScreen(int points, int coins, HighscoreUpdate highscoreUpdate = null)
		{
			string highscoreText = String.Empty;
			string highscoreResult = String.Empty;
			if (highscoreUpdate != null)
			{
				if (highscoreUpdate.Old == highscoreUpdate.New)
				{
					highscoreText = "Huidige Highscore:";
				}
				else
				{
					highscoreText = "Je hebt een nieuwe Highscore behaald:";
				}

				highscoreResult = highscoreUpdate.New.ToString();
			}

			spHighscore.text = highscoreText;
			spHighscoreResult.text = highscoreResult;
			spPoints.text = points.ToString();
			spCoins.text = coins.ToString();
			spTotal.text = (points + coins).ToString();

			DisplayEndScreen(GameType.Singleplayer);
		}

		public void DisplayMultiplayerCreateEndScreen(int points, int coins, bool success = true)
		{
			if (success)
			{
				mpCreateInfo.color = regularColor;
				mpCreateInfo.text = "Je uitdaging is verstuurd.";
			}
			else
			{
				mpCreateInfo.color = errorColor;
				mpCreateInfo.text = "Uitdaging kon niet verstuurd worden.";
			}

			mpCreatePoints.text = points.ToString();
			mpCreateCoins.text = coins.ToString();
			mpCreateTotal.text = (points + coins).ToString();

			DisplayEndScreen(GameType.MultiplayerCreate);
		}

		public void DisplayMultiplayerChallengeEndScreen(Match match, int points, int coins, bool success = true)
		{
			mpChallengePoints.text = points.ToString();
			mpChallengeCoins.text = coins.ToString();
			mpChallengeTotal.text = (points + coins).ToString();
			mpChallengeFriendTotal.text = match.CreatorScore.ToString();

			if (success)
			{
				mpErrorMessage.text = String.Empty;
				switch (match.Winner)
				{
					case MatchWinner.User:
						mpChallengeMatchResult.color = winColor;
						mpChallengeMatchResult.text = "GEWONNEN";
						break;
					case MatchWinner.Opponent:
						mpChallengeMatchResult.color = lossColor;
						mpChallengeMatchResult.text = "VERLOREN";
						break;
					case MatchWinner.Draw:
						mpChallengeMatchResult.color = drawColor;
						mpChallengeMatchResult.text = "GELIJKSPEL";
						break;
					default:
						throw new InvalidOperationException("Invalid winner.");
				}
			}
			else
			{
				mpErrorMessage.text = "Resultaat kon niet verstuurd worden.";
				mpChallengeMatchResult.text = String.Empty;
			}

			DisplayEndScreen(GameType.MultiplayerChallenge);
		}

		private void DisplayEndScreen(GameType gameType)
		{
			endScreenPanel.SetActive(true);

			switch (gameType)
			{
				case GameType.Singleplayer:
					singleplayerPanel.SetActive(true);
					break;
				case GameType.MultiplayerCreate:
					multiplayerCreatePanel.SetActive(true);
					break;
				case GameType.MultiplayerChallenge:
					multiplayerChallengePanel.SetActive(true);
					break;
				default:
					throw new InvalidOperationException("Invalid GameType provided.");
			}
		}

		public void ActivateInhalerButton()
		{
			inhalerButton.GetComponent<Button>().interactable = false;
			GameManager.Instance.Player.ActivateInhaler(PickupController.INHALER_TIME, PickupController.INHALER_SPEED);
		}

		public void PauseButton()
		{
			if (GameManager.Instance.Paused)
			{
				GameManager.Instance.Unpause();
				pauseText.text = "ll";
			}
			else
			{
				GameManager.Instance.Pause();
				pauseText.text = ">";
			}
		}

		public void ReturnToMainMenuButton()
		{
			SceneManager.LoadScene("MainStartMenu");
		}

		public void DisplayFactScreen()
		{
			endScreenPanel.SetActive(false);
			fact.text = facts.Pick() + " Ga voor meer informatie naar de Serious Request site.";
			factPanel.SetActive(true);
		}

		public void ToSeriousRequestWebsite()
		{
			Application.OpenURL("http://seriousrequest.3fm.nl/nieuws/detail/5351472/doel-3fm-serious-request-2016-longontsteking");
		}
	}
}

