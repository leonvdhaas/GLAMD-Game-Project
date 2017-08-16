using Assets.Scripts.Controllers;
using PlayfulSystems.ProgressBar;
using System;
using UnityEngine;
using UnityEngine.UI;

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
		private Image leftHeart;

		[SerializeField]
		private Image middleHeart;

		[SerializeField]
		private Image rightHeart;

		[SerializeField]
		private Vector2 position;

		[SerializeField]
		private Vector2 size;

		[SerializeField]
		private float spaceBetweenMeter;

		[SerializeField]
		private Texture2D emptyTexture;

		[SerializeField]
		private Texture2D fullTexture;

		[SerializeField]
		private GameObject coinDoublerBar;

		[SerializeField]
		private GameObject inhalerBar;

		[SerializeField]
		private GameObject slowmotionBar;

		//private float coinDoublerMeter;
		//private float inhalerMeter;
		//private float slowmotionMeter;

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
			inhalerBar
				.GetComponentInChildren<BarViewSizeImageFill>()
				.SetNumSteps(PickupController.MAX_NUMBER_OF_INHALERS);
			inhalerBar
				.GetComponentInChildren<BarViewValueText>()
				.SetMaxValue(PickupController.MAX_NUMBER_OF_INHALERS);
		}

		//private void OnGUI()
		//{
		//	float screenScaleX = Screen.width / 1200.0f;
		//	float screenScaleY = Screen.height / 800.0f;

		//	GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(screenScaleX, screenScaleY, 1));

		//	var meters = new float[] { inhalerMeter, coinDoublerMeter, slowmotionMeter };
		//	for (int i = 0; i < 3; i++)
		//	{
		//		//bar background
		//		GUI.BeginGroup(new Rect(position.x + spaceBetweenMeter * i, position.y, size.x, size.y));
		//		GUI.Box(new Rect(0, 0, size.x, size.y), emptyTexture);

		//		//bar filling part
		//		GUI.BeginGroup(new Rect(0, 0, size.x * meters[i], size.y));
		//		GUI.Box(new Rect(0, 0, size.x, size.y), fullTexture);
		//		GUI.EndGroup();
		//		GUI.EndGroup();
		//	}
		//}

		public void UpdateCoinDoublerMeter(float percentage)
		{
			targetCoinDoublerMeter = percentage;
		}

		public void UpdateInhalerMeter(float percentage)
		{
			targetInhalerMeter = percentage;
		}

		public void UpdateSlowmotionMeter(float percentage)
		{
			targetSlowmotionMeter = percentage;
		}

		private void FixedUpdate()
		{
			UpdateValue(inhaler, targetInhalerMeter);
			UpdateValue(coinDoubler, targetCoinDoublerMeter);
			UpdateValue(slowmotion, targetSlowmotionMeter);
		}

		private void UpdateValue(ProgressBarPro progressBar, float target)
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

		public void UpdateLives(int lives)
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

		public void UpdateCoins(int coins)
		{
			coinsText.text = String.Format("× {0}", coins.ToString().PadLeft(3, '0'));
		}
	}
}

