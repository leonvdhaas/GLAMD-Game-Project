using Assets.Scripts.Enumerations;
using Assets.Scripts.Managers;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Models.Pickups
{
	public class Heart
		: Pickup
	{
		private const int VALUE = 25;

		protected override void Activate(PlayerController player)
		{
			if (player.Lives < 3)
			{
				player.Lives++;
			}
			else
			{
				player.Points += VALUE;
			}

			SoundManager.Instance.PlaySoundEffect(Sound.Heart);
		}
	}
}
