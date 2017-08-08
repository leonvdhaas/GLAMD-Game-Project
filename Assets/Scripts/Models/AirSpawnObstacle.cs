namespace Assets.Scripts.Models
{
	public class AirSpawnObstacle
	{
		public int CoinChance { get; set; }

		public int NothingChance { get; set; }

		public void Fill(int coinChance, int nothingChance)
		{
			CoinChance = coinChance;
			NothingChance = nothingChance;
		}
	}
}
