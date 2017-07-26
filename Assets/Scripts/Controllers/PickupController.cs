using Assets.Scripts.Enumerations;
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
				switch (pickupType)
				{
					case Pickup.Diamond:
						break;
					case Pickup.Coin:
						break;
					case Pickup.Slowmotion:
						break;
					case Pickup.Inhaler:
						break;
					case Pickup.CoinDoubler:
						break;
					default:
						break;
				}
			}
		}
	}
}
