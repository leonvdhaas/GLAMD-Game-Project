using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using UnityEngine;

namespace Assets.Scripts.Triggers
{
	class ObstacleTrigger
		: MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			var player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				if (!player.IsInvincible)
					player.TakeObstacleDamage();
				gameObject.SetActive(false);
				//TODO: Add points
			}
		}
	}
}
