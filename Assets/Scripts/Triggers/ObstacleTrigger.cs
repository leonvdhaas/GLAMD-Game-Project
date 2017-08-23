using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Triggers
{
	public class ObstacleTrigger
		: MonoBehaviour
	{
		private const int OBSTACLE_DESTROY_SCORE = 3;

		private void OnTriggerEnter(Collider other)
		{
			var player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				if (player.InhalerPowerupActive)
				{
					SoundManager.Instance.PlaySoundEffect(Sound.BoxInvincibleBreak);
					player.Points += OBSTACLE_DESTROY_SCORE;
				}
				else
				{
					SoundManager.Instance.PlaySoundEffect(Sound.Thud);
					if (!player.IsInvincible)
					{
						player.TakeObstacleDamage();
					}
				}

				gameObject.SetActive(false);
				Destroy(gameObject);
			}
		}
	}
}
