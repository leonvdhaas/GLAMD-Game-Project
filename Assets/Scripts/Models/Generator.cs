namespace Assets.Scripts.Models
{
	public class Generator
	{
		public Generator()
		{
			SpawnObstacle = new SpawnObstacle();
			Spawn = new Spawn();
			AirSpawnObstacle = new AirSpawnObstacle();
		}

		public SpawnObstacle SpawnObstacle { get; set; }

		public Spawn Spawn { get; set; }

		public AirSpawnObstacle AirSpawnObstacle { get; set; }

		public void Fill(int obstacleChance, int pickupChance, int coinChance, int nothingChance)
		{
			SpawnObstacle.Fill(obstacleChance, pickupChance, coinChance, nothingChance);
			Spawn.Fill(pickupChance, coinChance, (100 - (pickupChance + coinChance)));
			AirSpawnObstacle.Fill(coinChance, (100 - coinChance));
		}
	}
}
