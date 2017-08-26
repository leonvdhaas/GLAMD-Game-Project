using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.Models.Pickups
{
	public abstract class Pickup
		: MonoBehaviour
	{
		private const float ROTATE_SPEED = 5;

		private void FixedUpdate()
		{
			transform.Rotate(Vector3.up * ROTATE_SPEED);
		}

		private void OnTriggerEnter(Collider other)
		{
			var player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				gameObject.SetActive(false);
				Activate(player);
				Destroy(gameObject);
			}
		}

		protected abstract void Activate(PlayerController player);
	}
}
