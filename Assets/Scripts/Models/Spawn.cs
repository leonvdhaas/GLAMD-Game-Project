namespace Assets.Scripts.Models
{
	public class Spawn
	{
		public int PickupChance { get; set; }
		
		public int CoinChance { get; set; }

		public int NothingChance { get; set; }

		public void Fill(int pickupChance, int coinChance, int nothingChance)
		{
			PickupChance = pickupChance;
			CoinChance = coinChance;
			NothingChance = nothingChance;
		}
	}
}
