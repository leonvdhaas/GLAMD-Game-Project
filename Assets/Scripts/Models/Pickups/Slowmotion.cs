using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;
using Assets.Scripts.Enumerations;

namespace Assets.Scripts.Models.Pickups
{
	public class Slowmotion
		: Pickup
	{
		private const float DURATION = 2.5f;
		private const float FACTOR = 0.5f;

		protected override void Activate(PlayerController player)
		{
			player.ActivateSlowmotion(DURATION, FACTOR);
			SoundManager.Instance.PlaySoundEffect(Sound.Slowmotion);
			player.ActivateParticleSystem(ParticleType.SlowMotion);
		}
	}
}
