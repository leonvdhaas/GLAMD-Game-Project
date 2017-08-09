using Assets.Scripts.Helpers;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
	public class GuiManager
		: MonoBehaviour
	{
		[SerializeField]
		private Text coinsText;
		[SerializeField]
		private Text pointsText;
		[SerializeField]
		private Image leftHeart;
		[SerializeField]
		private Image middleHeart;
		[SerializeField]
		private Image rightHeart;

		private void UpdatePoints(int points)
		{
			pointsText.text = points.ToString().PadLeft(6, '0');
		}

		private void UpdateLives(int lives)
		{
			switch (lives)
			{
				case 0:
					leftHeart.enabled = middleHeart.enabled = rightHeart.enabled = false;
					break;
				case 1:
					leftHeart.enabled = true;
					middleHeart.enabled = rightHeart.enabled = false;
					break;
				case 2:
					leftHeart.enabled = middleHeart.enabled = true;
					rightHeart.enabled = false;
					break;
				case 3:
					leftHeart.enabled = middleHeart.enabled = rightHeart.enabled = true;
					break;
				default:
					throw new InvalidOperationException();
			}
		}

		private void UpdateCoins(int coins)
		{
			coinsText.text = String.Format("× {0}", coins.ToString().PadLeft('0', '3'));
		}
	}
}
