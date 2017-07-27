using Assets.Scripts.Enumerations;
using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		private const ushort MAX_NUMBER_OF_INHALERS = 6;

		private const float SLOWMOTION_TIME = 5.0f;

		private const float SLOWMOTION_FACTOR = 0.5f;

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
						break;
					case Pickup.Slowmotion:
						Time.timeScale = SLOWMOTION_FACTOR;
						StartCoroutine(CoroutineHelper.Delay(SLOWMOTION_TIME, () => Time.timeScale = 1.0f));
						break;
					case Pickup.Inhaler:
						if (!(player.Inhalers >= MAX_NUMBER_OF_INHALERS))
							player.Inhalers++;
						break;
					case Pickup.CoinDoubler:
						player.IsCoinDoublerActive = true;
						StartCoroutine(CoroutineHelper.Delay(2.5f, () =>
						{
							player.IsCoinDoublerActive = false;
						}));
						break;
					default:
						throw new InvalidOperationException("Invalid Pickup type.");
				}

				Destroy(gameObject);
			}
		}
	}
}
