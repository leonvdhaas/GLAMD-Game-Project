using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Assets.Scripts.Extensions;
using Assets.Scripts.Models;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Tiles;

namespace Assets.Scripts.Utilities
{
	public class TilePopulator
		: MonoBehaviour
	{
		private class TilePopulation
		{
			public TilePopulation(StraightTile tile)
			{
				Tile = tile;

				Box1 = new List<SpawnObject[,]>();
				Box2 = new List<SpawnObject[,]>();
				Box3 = new List<SpawnObject[,]>();
				Box4 = new List<SpawnObject[,]>();
				Box5 = new List<SpawnObject[,]>();

				SpawnObstacles = new SpawnObject[1, 3];
				GroundSpawns = new SpawnObject[3, 5];
				JumpOverObstacles = new SpawnObject[3, 5];
				AirSpawns = new SpawnObject[1, 3];
			}

			public StraightTile Tile { get; set; }

			// Blocks of gameplay
			public List<SpawnObject[,]> Box1 { get; set; }

			public List<SpawnObject[,]> Box2 { get; set; }

			public List<SpawnObject[,]> Box3 { get; set; }

			public List<SpawnObject[,]> Box4 { get; set; }

			public List<SpawnObject[,]> Box5 { get; set; }

			// SpawnPositions
			public SpawnObject[,] SpawnObstacles { get; set; }

			public SpawnObject[,] GroundSpawns { get; set; }

			public SpawnObject[,] JumpOverObstacles { get; set; }

			public SpawnObject[,] AirSpawns { get; set; }

			public int PickupCount { get; set; }
		}

		// Obstacles
		[SerializeField]
		private GameObject cartoonCar;
		[SerializeField]
		private GameObject jeep;
		[SerializeField]
		public GameObject box;
		[SerializeField]
		public GameObject hotdogTruck;
		[SerializeField]
		public GameObject pizzaTruck;

		// Pickups
		[SerializeField]
		public GameObject doubleCoins;
		[SerializeField]
		public GameObject inhaler;
		[SerializeField]
		public GameObject slowmotion;
		[SerializeField]
		public GameObject heart;

		// Coins
		[SerializeField]
		public GameObject coin;
		[SerializeField]
		public GameObject diamond;

		// Which ones are unnecessary?

		//private bool bigObstacle = false;
		//private int coinBowCount = 0;
		//private int truckCount = 0;
		//private int bigType;
		//private int spawnCounter = 0;
		//private int listCounter = 0;
		//private int obstacleChance, pickupChance, coinChance, nothingChance;
		//private object[] obstacleType = new object[5];

		private void Awake()
		{
			Chances = new Generator();
			StartCoroutine(CoroutineHelper.For(
				15,
				() => 20,
				i => i <= 45,
				(ref int i) => i++,
				i => Chances.Fill(i, 20, 10, 70 - i)));
		}

		// Chance Generator
		public static Generator Chances { get; set; }

		private GameObject PickupChance()
		{
			return RandomUtilities.WeightedPick(
				heart.ToWeightedItem(1),
				slowmotion.ToWeightedItem(25),
				inhaler.ToWeightedItem(100),
				doubleCoins.ToWeightedItem(150));
		}

		private GameObject CoinChance()
		{
			return RandomUtilities.WeightedPick(
				diamond.ToWeightedItem(1),
				coin.ToWeightedItem(20));
		}

		private GameObject ObstacleChance()
		{
			return RandomUtilities.WeightedPick(
				box.ToWeightedItem(7),
				cartoonCar.ToWeightedItem(5),
				pizzaTruck.ToWeightedItem(2));
		}

		public void Populate(StraightTile tile)
		{
			var tilePopulation = new TilePopulation(tile);
			Assign(tilePopulation);
		}

		private void Assign(TilePopulation tilePopulation)
		{
			// assign each spawnobject to corresponding transform

			StartCoroutine(CoroutineHelper.For(
				0.1f,
				() => 1,
				o => o < 6,
				(ref int o) => o++,
				o =>
				{
					for (int i = 0; i < 3; i++)
					{
						tilePopulation.SpawnObstacles[0, i] = new SpawnObject();
						tilePopulation.SpawnObstacles[0, i].Location = tilePopulation.Tile.transform.Find(String.Format("Spawner/Block {0}/Obstacles/SpawnObstacle{1}", o, i + 1));
						tilePopulation.AirSpawns[0, i] = new SpawnObject();
						tilePopulation.AirSpawns[0, i].Location = tilePopulation.Tile.transform.Find(String.Format("Spawner/Block {0}/AirSpawns/AirSpawn{1}", o, i + 1));

						for (int e = 0; e < 5; e++)
						{
							tilePopulation.GroundSpawns[i, e] = new SpawnObject();
							tilePopulation.GroundSpawns[i, e].Location = tilePopulation.Tile.transform.Find(String.Format("Spawner/Block {0}/GroundSpawns/Line {1}/GroundSpawn{2}", o, i + 1, e + 1));
							tilePopulation.JumpOverObstacles[i, e] = new SpawnObject();
							tilePopulation.JumpOverObstacles[i, e].Location = tilePopulation.Tile.transform.Find(String.Format("Spawner/Block {0}/JumpOverObstacle/Line {1}/AirObstacleSpawn{2}", o, i + 1, e + 1));
						}
					}

					Fill(tilePopulation);
				}));
		}

		private void Fill(TilePopulation tilePopulation)
		{
			foreach (SpawnObject spawnObstacles in tilePopulation.SpawnObstacles)
			{
				if (RandomUtilities.PercentageChance(Chances.SpawnObstacle.ObstacleChance))
				{
					spawnObstacles.Object = ObstacleChance();
				}
				else if (RandomUtilities.PercentageChance(Chances.SpawnObstacle.PickupChance))
				{
					spawnObstacles.Object = GetPickup(tilePopulation, spawnObstacles);
				}
				else if (RandomUtilities.PercentageChance(Chances.SpawnObstacle.CoinChance))
				{
					spawnObstacles.Object = CoinChance();
				}
				else
				{
					spawnObstacles.Alive = false;
				}
			}

			var spawns = tilePopulation.GroundSpawns
				.Cast<SpawnObject>()
				.Concat(tilePopulation.AirSpawns.Cast<SpawnObject>());
			foreach (SpawnObject spawn in spawns)
			{
				SetPickupOrPowerup(tilePopulation, spawn);
			}

			for (int i = 0; i < 3; i++)
			{
				if (tilePopulation.SpawnObstacles[0, i].Object == box)
				{
					for (int a = 0; a < 5; a++)
					{
						tilePopulation.JumpOverObstacles[i, a].Object = CoinChance();
					}

					tilePopulation.AirSpawns[0, i].Alive = false;
				}
				else
				{
					for (int a = 0; a < 5; a++)
					{
						tilePopulation.JumpOverObstacles[i, a].Alive = false;
					}
				}
			}

			Spawn(tilePopulation);
		}

		private void Spawn(TilePopulation tilePopulation)
		{
			var spawns = tilePopulation.SpawnObstacles
				.Cast<SpawnObject>()
				.Concat(tilePopulation.GroundSpawns.Cast<SpawnObject>())
				.Concat(tilePopulation.AirSpawns.Cast<SpawnObject>())
				.Concat(tilePopulation.JumpOverObstacles.Cast<SpawnObject>());
			foreach (var spawn in spawns)
			{
				if (spawn != null && spawn.Alive && spawn.Object != null && spawn.Location != null)
				{
					Instantiate(
							spawn.Object,
							spawn.Location.position,
							spawn.Location.rotation,
							tilePopulation.Tile.transform.Find("Spawner"));
				}
			}

			const string defaultName = "New Game Object";
			foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject)).Where(x => x.name == defaultName))
			{
				Destroy(gameObject);
			}
		}

		private void SetPickupOrPowerup(TilePopulation tilePopulation, SpawnObject pickup)
		{
			if (RandomUtilities.PercentageChance(Chances.Spawn.PickupChance))
			{
				pickup.Object = GetPickup(tilePopulation, pickup);
			}
			else if (RandomUtilities.PercentageChance(Chances.Spawn.CoinChance))
			{
				pickup.Object = CoinChance();
			}
			else
			{
				pickup.Alive = false;
			}
		}

		private GameObject GetPickup(TilePopulation tilePopulation, SpawnObject pickup)
		{
			var retval = new GameObject();

			// How many pickups per straight tile.
			if (tilePopulation.PickupCount < 1)
			{
				retval = PickupChance();
				tilePopulation.PickupCount++;
			}
			else
			{
				pickup.Alive = false;
			}

			return retval;
		}
	}
}
