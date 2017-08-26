using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Models.Pickups
{
	public class Inhaler
		: Pickup
	{
		public const float DURATION = 5;
		public const float SPEED_BONUS = 10;
		public const int MAX_AMOUNT = 5;
		private const int VALUE = 10;

		protected override void Activate(PlayerController player)
		{
			if (player.Inhalers < MAX_AMOUNT)
			{
				player.Inhalers++;
			}
			else
			{
				player.Points += VALUE;
			}

			SoundManager.Instance.PlaySoundEffect(Sound.Inhaler);
		}
	}
}
