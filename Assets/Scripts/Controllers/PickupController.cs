using Assets.Scripts.Enumerations;
using Assets.Scripts.Helpers;
using Assets.Scripts.Managers;
using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
	public class PickupController
		: MonoBehaviour
	{
		private const int DIAMOND_VALUE = 5;

		[SerializeField]
		private float rotateSpeed;

		[SerializeField]
		private Pickup pickupType;

		public const ushort MAX_NUMBER_OF_INHALERS = 6;

		private const float SLOWMOTION_TIME = 5.0f;

		private const float SLOWMOTION_FACTOR = 0.5f;

		private const float COINDOUBLER_TIME = 2.5f;

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
						break;
					case Pickup.Coin:
						player.Coins += 1 * multiplier;
						SoundManager.Instance.PlaySound(Sound.Coin);
						break;
					case Pickup.Slowmotion:
						player.SetSlowmotionActive(SLOWMOTION_TIME, SLOWMOTION_FACTOR);
						SoundManager.Instance.PlaySound(Sound.Slowmotion);
						break;
					case Pickup.Inhaler:
						if (player.Inhalers < MAX_NUMBER_OF_INHALERS)
						{
							player.Inhalers++;
						}
						else if (player.IsInvincible)
						{
							// TO-DO: add points
						}
						SoundManager.Instance.PlaySound(Sound.Inhaler);
						break;
					case Pickup.CoinDoubler:
						player.SetCoinDoublerActive(COINDOUBLER_TIME);
						break;
					default:
						throw new InvalidOperationException("Invalid Pickup type.");
				}

				Destroy(gameObject);
			}
		}
	}
}
