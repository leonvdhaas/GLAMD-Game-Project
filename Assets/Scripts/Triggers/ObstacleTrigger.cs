using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Triggers
{
	public class ObstacleTrigger
		: MonoBehaviour
	{
		private const int OBSTACLE_DESTROY_SCORE = 5;

		private void OnTriggerEnter(Collider other)
		{
			var player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				if (!player.IsInvincible)
				{
					SoundManager.Instance.PlaySound(Sound.Thud);
					player.TakeObstacleDamage();
					player.Points += OBSTACLE_DESTROY_SCORE;
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
