using Assets.Scripts.Enumerations;
using Assets.Scripts.Helpers;
using Assets.Scripts.Managers;
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
						break;
					case Pickup.Inhaler:
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
