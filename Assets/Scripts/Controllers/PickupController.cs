using Assets.Scripts.Enumerations;
using Assets.Scripts.Managers;
using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
	public class PickupController
		: MonoBehaviour
	{
		public const ushort MAX_NUMBER_OF_INHALERS = 6;
		private const float COINDOUBLER_TIME = 2.5f;
		private const float SLOWMOTION_TIME = 2.5f;
		private const float SLOWMOTION_FACTOR = 0.5f;
		private const int HEART_VALUE = 25;
		private const int INHALER_VALUE = 10;
		private const int DIAMOND_VALUE = 5;
		private const int COIN_VALUE = 1;

		[SerializeField]
		private float rotateSpeed;

		[SerializeField]
		private Pickup pickupType;

		private void FixedUpdate()
		{
			transform.Rotate(Vector3.up * rotateSpeed);
		}

		private void OnTriggerEnter(Collider other)
		{
			var player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				int multiplier = player.IsCoinDoublerActive ? 2 : 1;

				switch (pickupType)
				{
					case Pickup.Diamond:
						player.Coins += DIAMOND_VALUE * multiplier;
						SoundManager.Instance.PlaySoundEffect(Sound.Diamond);
						break;
					case Pickup.Coin:
						player.Coins += COIN_VALUE * multiplier;
						SoundManager.Instance.PlaySoundEffect(Sound.Coin);
						break;
					case Pickup.Slowmotion:
						player.ActivateSlowmotion(SLOWMOTION_TIME, SLOWMOTION_FACTOR);
						SoundManager.Instance.PlaySoundEffect(Sound.Slowmotion);
						break;
					case Pickup.Inhaler:
						if (player.Inhalers < MAX_NUMBER_OF_INHALERS)
						{
							player.Inhalers++;
						}
						else
						{
							player.Points += INHALER_VALUE;
						}

						SoundManager.Instance.PlaySoundEffect(Sound.Inhaler);
						break;
					case Pickup.CoinDoubler:
						player.ActivateCoinDoubler(COINDOUBLER_TIME);
						SoundManager.Instance.PlaySoundEffect(Sound.CoinDoubler);
						break;
					case Pickup.Heart:
						if (player.Lives < 3)
						{
							player.Lives++;
						}
						else
						{
							player.Points += HEART_VALUE;
						}

						SoundManager.Instance.PlaySoundEffect(Sound.Heart);
						break;
					default:
						throw new InvalidOperationException("Invalid Pickup type.");
				}

				gameObject.SetActive(false);
				Destroy(gameObject);
			}
		}
	}
}
