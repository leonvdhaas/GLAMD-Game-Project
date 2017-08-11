using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
	public class GuiManager
		: MonoBehaviour
	{
		private const float METER_DELTA = 0.01f;

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

		private float inhalerMeter;
		private float coinDoublerMeter;
		private float slowmotionMeter;

		private float targetSlowmotionMeter;
		private float targetCoinDoublerMeter;
		private float targetInhalerMeter;

		private void Start()
		{
			GameManager.Instance.GuiManager = this;
		}

		private void OnGUI()
		{
			float screenScaleX = Screen.width / 1200.0f;
			float screenScaleY = Screen.height / 800.0f;

			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(screenScaleX, screenScaleY, 1));

			var meters = new float[] { inhalerMeter, coinDoublerMeter, slowmotionMeter };
			for (int i = 0; i < 3; i++)
			{
				//bar background
				GUI.BeginGroup(new Rect(position.x + spaceBetweenMeter * i, position.y, size.x, size.y));
				GUI.Box(new Rect(0, 0, size.x, size.y), emptyTexture);

				//bar filling part
				GUI.BeginGroup(new Rect(0, 0, size.x * meters[i], size.y));
				GUI.Box(new Rect(0, 0, size.x, size.y), fullTexture);
				GUI.EndGroup();
				GUI.EndGroup();
			}
		}

		public void UpdateInhalerMeter(float percentage)
		{
			targetInhalerMeter = percentage;
		}

		public void UpdateCoinDoublerMeter(float percentage)
		{
			targetCoinDoublerMeter = percentage;
		}

		public void UpdateSlowmotionMeter(float percentage)
		{
			targetSlowmotionMeter = percentage;
		}

		private void FixedUpdate()
		{
			UpdateValue(ref inhalerMeter, targetInhalerMeter);
			UpdateValue(ref coinDoublerMeter, targetCoinDoublerMeter);
			UpdateValue(ref slowmotionMeter, targetSlowmotionMeter);
		}

		private void UpdateValue(ref float current, float target)
		{
			if (!Mathf.Approximately(current, target))
			{
				current = Mathf.MoveTowards(current, target, METER_DELTA);
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

