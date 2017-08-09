namespace Assets.Scripts.Models
{
	public class SpawnObstacle
	{
		public int ObstacleChance { get; set; }

		public int PickupChance { get; set; }

		public int CoinChance { get; set; }

		public int NothingChance { get; set; }

		public void Fill(int obstacleChance, int pickupChance, int coinChance, int nothingChance)
		{
			ObstacleChance = obstacleChance;
			PickupChance = pickupChance;
			CoinChance = coinChance;
			NothingChance = nothingChance;
		}
	}
}
