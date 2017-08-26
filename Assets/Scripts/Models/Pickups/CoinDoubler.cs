using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Models.Pickups
{
	public class CoinDoubler
		: Pickup
	{
		private const float DURATION = 2.5f;

		protected override void Activate(PlayerController player)
		{
			player.ActivateCoinDoubler(DURATION);
			SoundManager.Instance.PlaySoundEffect(Sound.CoinDoubler);
		}
	}
}
