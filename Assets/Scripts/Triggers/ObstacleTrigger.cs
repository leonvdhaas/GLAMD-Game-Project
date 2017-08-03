using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Managers;
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
				{
					SoundManager.Instance.PlaySound(Sound.Thud);
					player.TakeObstacleDamage();
				}
				else
				{
					SoundManager.Instance.PlaySound(Sound.BoxInvincibleBreak);
				}
				gameObject.SetActive(false);
				//TODO: Add points
			}
		}
	}
}
