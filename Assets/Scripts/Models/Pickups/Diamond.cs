using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Models.Pickups
{
	public class Diamond
		: Pickup
	{
		private const int VALUE = 5;

		protected override void Activate(PlayerController player)
		{
			player.Coins += (player.IsCoinDoublerActive ? 2 : 1) * VALUE;
			SoundManager.Instance.PlaySoundEffect(Sound.Diamond);
		}
	}
}
