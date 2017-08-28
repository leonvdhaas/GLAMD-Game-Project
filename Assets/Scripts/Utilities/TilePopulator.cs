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
				Blocks = new SpawnObject[5][][,];
			}

			public StraightTile Tile { get; set; }

			// Blocks of gameplay
			public SpawnObject[][][,] Blocks { get; set; }

			// SpawnPositions
			public SpawnObject[,] SpawnObstacles { get; set; }
			public SpawnObject[,] GroundSpawns { get; set; }
			public SpawnObject[,] JumpOverObstacles { get; set; }
			public SpawnObject[,] AirSpawns { get; set; }
			public SpawnObject[,] BoxCoins { get; set; }
			public SpawnObject[,] Boxes { get; set; }
			public SpawnObject MoveTrigger { get; set; }
			public int PickupCount { get; set; }
		}

		// Obstacles
		[SerializeField]
		private GameObject container;
		[SerializeField]
		private GameObject cartoonCar;
		[SerializeField]
		private GameObject jeep;
		[SerializeField]
		private GameObject box;
		[SerializeField]
		private GameObject hotdogTruck;
		[SerializeField]
		private GameObject pizzaTruck;
		[SerializeField]
		private GameObject cone;
		[SerializeField]
		private GameObject bin;
		[SerializeField]
		private GameObject roadBarrier;

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

		// MoveTrigger
		[SerializeField]
		public GameObject moveTrigger;

		// Arraynumbers
		private int spawnobstacles = 0;
		private int airspawns = 1;
		private int boxes = 2;
		private int groundspawns = 3;
		private int jumpoverobstacles = 4;
		private int boxcoins = 5;

		// Chance Generator
		public static Generator Chances { get; set; }

		// RoadBarrier
		private int roadbarier;

		// BoxRow
		private int rowStartBlock;
		private int rowLineStart;
		private bool rowBoxes;

		// Moving car
		private int movingStartBlock;
		private int movingLineStart;
		private bool movingCar = false;

		private int pickupCount = 0;
		private bool bigObstacle = false;

		private int guideline;  // lijn met muntjes voor de speler om te volgen mits een stuk baan leeg is

		private void Awake()
		{
			Chances = new Generator();
			//StartCoroutine(CoroutineHelper.For(
			//    15,
			//    () => 20,
			//    i => i <= 45,
			//    (ref int i) => i += 2,
			//    i => Chances.Fill(i, 20, 10, 70 - i)));

			Chances.Fill(55, 20, 20, 05);
		}

		private GameObject JumpableObstacle()
		{
			return RandomUtilities.Pick(cone, bin, container);
		}

		private SpawnObject SetPickupOrPowerup(TilePopulation tilePopulation, SpawnObject pickup)
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
			return pickup;
		}

		private SpawnObject BoxPickupOrPowerup(TilePopulation tilePopulation, SpawnObject pickup)
		{
			if (RandomUtilities.PercentageChance(Chances.Spawn.PickupChance))
			{
				pickup.Object = GetPickup(tilePopulation, pickup);
			}
			else
			{
				pickup.Object = CoinChance();
			}
			return pickup;
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

		private bool rowchance()
		{
			return RandomUtilities.PercentageChance(Chances.SpawnObstacle.ObstacleChance / 4); // kans op rij dozen
		}

		private bool movingchance()
		{
			return RandomUtilities.PercentageChance(Chances.SpawnObstacle.ObstacleChance / 2); // kans op een bewegende auto
		}

		private GameObject CoinChance()
		{
			return RandomUtilities.WeightedPick(
				diamond.ToWeightedItem(2),
				coin.ToWeightedItem(20));
		}

		private GameObject PickupChance()
		{
			return RandomUtilities.WeightedPick(
				heart.ToWeightedItem(1),
				slowmotion.ToWeightedItem(25),
				inhaler.ToWeightedItem(100),
				doubleCoins.ToWeightedItem(150));
		}

		private GameObject ObstacleChance()
		{
			return RandomUtilities.WeightedPick(
			JumpableObstacle().ToWeightedItem(30),
			pizzaTruck.ToWeightedItem(1));
		}

		private GameObject ObstacleOrBoth()
		{
			if (rowchance() && !rowBoxes)
			{
				rowBoxes = true;
				return box;
			}
			else if (movingchance() && !movingCar)
			{
				movingCar = true;
				return cartoonCar;
			}
			else
			{
				return ObstacleChance();
			}
		}

		private GameObject ObstacleOrMoving()
		{
			if (movingchance() && !movingCar)
			{
				movingCar = true;
				return cartoonCar;
			}
			else
			{
				return ObstacleChance();
			}
		}

		private GameObject ObstacleOrRow()
		{
			if (rowchance() && !rowBoxes)
			{
				return box;
			}
			else
			{
				return ObstacleChance();
			}
		}

		private int RowLength()
		{
			int length = 0;
			switch (rowStartBlock)
			{
				case 1:
					length = RandomUtilities.Pick(1, 2, 3);
					break;
				case 2:
					length = RandomUtilities.Pick(1, 2);
					break;
				case 3:
					length = 1;
					break;
			}
			return length;
		}

		private int MovingLength()
		{
			int length = 0;
			switch (movingStartBlock)
			{
				case 4:
					length = RandomUtilities.Pick(1, 2, 3);
					break;
				case 3:
					length = RandomUtilities.Pick(1, 2);
					break;
				case 2:
					length = RandomUtilities.Pick(1);
					break;
			}
			return length;
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
					SpawnObject[][,] Block = new SpawnObject[6][,];
					tilePopulation.SpawnObstacles = new SpawnObject[1, 3];
					tilePopulation.GroundSpawns = new SpawnObject[3, 5];
					tilePopulation.JumpOverObstacles = new SpawnObject[3, 5];
					tilePopulation.BoxCoins = new SpawnObject[3, 6];
					tilePopulation.Boxes = new SpawnObject[3, 3];
					tilePopulation.AirSpawns = new SpawnObject[1, 3];

					for (int i = 0; i < 3; i++)
					{
						tilePopulation.SpawnObstacles[0, i] = new SpawnObject();
						tilePopulation.SpawnObstacles[0, i].Location = tilePopulation.Tile.transform.Find(String.Format("Spawner/Block {0}/Obstacles/SpawnObstacle{1}", o, i + 1));
						tilePopulation.AirSpawns[0, i] = new SpawnObject();
						tilePopulation.AirSpawns[0, i].Location = tilePopulation.Tile.transform.Find(String.Format("Spawner/Block {0}/AirSpawns/AirSpawn{1}", o, i + 1));

						for (int e = 0; e < 3; e++)
						{
							tilePopulation.Boxes[i, e] = new SpawnObject();
							tilePopulation.Boxes[i, e].Location = tilePopulation.Tile.transform.Find(String.Format("Spawner/Block {0}/Boxes/Line {1}/Box{2}", o, i + 1, e + 1));

						}

						for (int e = 0; e < 5; e++)
						{
							tilePopulation.GroundSpawns[i, e] = new SpawnObject();
							tilePopulation.GroundSpawns[i, e].Location = tilePopulation.Tile.transform.Find(String.Format("Spawner/Block {0}/GroundSpawns/Line {1}/GroundSpawn{2}", o, i + 1, e + 1));
							tilePopulation.JumpOverObstacles[i, e] = new SpawnObject();
							tilePopulation.JumpOverObstacles[i, e].Location = tilePopulation.Tile.transform.Find(String.Format("Spawner/Block {0}/JumpOverObstacle/Line {1}/AirObstacleSpawn{2}", o, i + 1, e + 1));
						}

						if (o >= 2 && o <= 4)
						{
							for (int e = 0; e < 6; e++)
							{
								tilePopulation.BoxCoins[i, e] = new SpawnObject();
								tilePopulation.BoxCoins[i, e].Location = tilePopulation.Tile.transform.Find(String.Format("Spawner/Block {0}/BoxCoins/Line {1}/BoxCoin{2}", o, i + 1, e + 1));
							}
						}
					}

					tilePopulation.MoveTrigger = new SpawnObject();

					Block[0] = tilePopulation.SpawnObstacles;
					Block[1] = tilePopulation.AirSpawns;
					Block[2] = tilePopulation.Boxes;
					Block[3] = tilePopulation.GroundSpawns;
					Block[4] = tilePopulation.JumpOverObstacles;
					Block[5] = tilePopulation.BoxCoins;

					tilePopulation.Blocks[o - 1] = Block;

					if (o == 5)
					{
						FillStatic(tilePopulation);
					}
				}));
		}

		private void FillStatic(TilePopulation tilePopulation)
		{
			rowBoxes = false;
			movingCar = false;

			for (int e = 0; e < 5; e++)
			{
				if (e % 1 == 0)
				{
					guideline = RandomUtilities.Pick(0, 1, 2);
				}

				foreach (SpawnObject spawnObstacle in tilePopulation.Blocks[e][spawnobstacles])
				{
					if (RandomUtilities.PercentageChance(Chances.SpawnObstacle.ObstacleChance))
					{
						if (e == 0)
						{
							spawnObstacle.Object = ObstacleChance();
						}
						else if (e > 0 && e < 4)
						{
							if (e > 2)
							{
								spawnObstacle.Object = ObstacleOrBoth();
							}
							else
							{
								spawnObstacle.Object = ObstacleOrRow();
							}
						}
						else
						{
							spawnObstacle.Object = ObstacleOrMoving();
						}

						if (spawnObstacle.Object == box && !rowBoxes)
						{
							rowBoxes = true;
							rowStartBlock = e;
						}

						if (spawnObstacle.Object == cartoonCar && !movingCar)
						{
							movingCar = true;
							movingStartBlock = e;
						}
					}
					else if (RandomUtilities.PercentageChance(Chances.SpawnObstacle.PickupChance))
					{
						spawnObstacle.Object = GetPickup(tilePopulation, spawnObstacle);
					}
					else
					{
						spawnObstacle.Alive = false;
					}
				}

				for (int i = 0; i < 3; i++) // jump or not jump over obstacle, pickups
				{
					SpawnObject obstacle = tilePopulation.Blocks[e][spawnobstacles][0, i];
					if ((obstacle.Object == cone || obstacle.Object == bin || obstacle.Object == container) && (obstacle.Alive))
					{
						if (RandomUtilities.PercentageChance(Chances.Spawn.CoinChance))
						{
							for (int a = 0; a < 5; a++)
							{
								tilePopulation.Blocks[e][jumpoverobstacles][i, a].Object = CoinChance();
								tilePopulation.Blocks[e][groundspawns][guideline, a].Alive = false;
							}

							tilePopulation.Blocks[e][airspawns][0, i].Alive = false;
						}
						else if ((RandomUtilities.PercentageChance(Chances.Spawn.PickupChance)))
						{
							for (int a = 0; a < 5; a++)
							{
								tilePopulation.Blocks[e][jumpoverobstacles][i, a].Alive = false;
								tilePopulation.Blocks[e][groundspawns][guideline, a].Alive = false;
							}

							tilePopulation.Blocks[e][airspawns][0, i] = SetPickupOrPowerup(tilePopulation, tilePopulation.Blocks[e][airspawns][0, i]);
						}
					}
					else if (!obstacle.Alive && obstacle.Object == null)
					{
						for (int b = 0; b < tilePopulation.Blocks[e][jumpoverobstacles].GetLength(1); b++)
						{
							tilePopulation.Blocks[e][jumpoverobstacles][i, b].Alive = false;
						}

						tilePopulation.Blocks[e][airspawns][0, i].Alive = false;
					}
				}
				if (!tilePopulation.Blocks[e][spawnobstacles][0, guideline].Alive && tilePopulation.Blocks[e][spawnobstacles][0, guideline].Object == null &&
					RandomUtilities.PercentageChance(Chances.Spawn.CoinChance))
				{
					for (int b = 0; b < tilePopulation.Blocks[e][groundspawns].GetLength(1); b++)
					{
						tilePopulation.Blocks[e][groundspawns][guideline, b].Object = CoinChance();
						tilePopulation.Blocks[e][spawnobstacles][0, guideline].Alive = true;
						tilePopulation.Blocks[e][spawnobstacles][0, guideline].Object = CoinChance();
					}
				}
			}

			if (rowBoxes)
			{
				FillBoxRows(tilePopulation);
			}

			if (movingCar)
			{
				FillMoving(tilePopulation);
			}

			FillBigObstacles(tilePopulation);
			Spawn(tilePopulation);
		}

		private void FillBoxRows(TilePopulation tilePopulation)
		{
			for (int e = 0; e < 5; e++)
			{
				for (int i = 0; i < 3; i++)
				{
					if (tilePopulation.Blocks[e][spawnobstacles][0, i].Object == box) { rowLineStart = i; }
				}
			}

			int rowLength = RowLength();

			for (int block = rowStartBlock; block < (rowStartBlock + rowLength); block++)
			{
				for (int boxcount = 0; boxcount < 3; boxcount++)
				{
					tilePopulation.Blocks[block][boxes][rowLineStart, boxcount].Object = box;
					tilePopulation.Blocks[block][boxes][rowLineStart, boxcount].Alive = true;
				}

				tilePopulation.Blocks[block][spawnobstacles][0, rowLineStart].Alive = false;
				tilePopulation.Blocks[block][airspawns][0, rowLineStart].Alive = false;

				for (int groundspawncount = 0; groundspawncount < 5; groundspawncount++)
				{
					tilePopulation.Blocks[block][groundspawns][rowLineStart, groundspawncount].Alive = false;
					tilePopulation.Blocks[block][jumpoverobstacles][rowLineStart, groundspawncount].Alive = false;
				}

				for (int boxcoincount = 0; boxcoincount < 6; boxcoincount++)
				{
					if (tilePopulation.Blocks[block][boxcoins][rowLineStart, boxcoincount] != null)
					{
						tilePopulation.Blocks[block][boxcoins][rowLineStart, boxcoincount] = BoxPickupOrPowerup(tilePopulation, tilePopulation.Blocks[block][boxcoins][rowLineStart, boxcoincount]);
						tilePopulation.Blocks[block][boxcoins][rowLineStart, boxcoincount].Alive = true;
					}
				}
			}

			tilePopulation.Blocks[rowStartBlock - 1][spawnobstacles][0, rowLineStart].Alive = false;
			tilePopulation.Blocks[rowStartBlock + rowLength][spawnobstacles][0, rowLineStart].Alive = false;
			tilePopulation.Blocks[rowStartBlock - 1][airspawns][0, rowLineStart].Alive = false;
			tilePopulation.Blocks[rowStartBlock + rowLength][airspawns][0, rowLineStart].Alive = false;

			for (int airspawncount = 0; airspawncount < 5; airspawncount++)
			{
				tilePopulation.Blocks[rowStartBlock - 1][jumpoverobstacles][rowLineStart, airspawncount].Alive = false;
				tilePopulation.Blocks[rowStartBlock + rowLength][jumpoverobstacles][rowLineStart, airspawncount].Alive = false;
			}
		}

		private void FillMoving(TilePopulation tilePopulation)
		{
			bool found = false;
			for (int b = 4; b > 1; b--)
			{
				for (int i = 0; i < 3; i++)
				{
					if (tilePopulation.Blocks[b][spawnobstacles][0, i].Object == cartoonCar && i != rowLineStart && !found)
					{
						movingLineStart = i;
						movingStartBlock = b;
						tilePopulation.Blocks[b][spawnobstacles][0, i].Move = true;
						found = true;
					}
					else if (tilePopulation.Blocks[b][spawnobstacles][0, i].Object == cartoonCar && found)
					{
						tilePopulation.Blocks[b][spawnobstacles][0, i].Object = JumpableObstacle();
					}
				}
			}

			int moveLength = MovingLength();

			if (found)
			{
				tilePopulation.MoveTrigger.Location = tilePopulation.Tile.transform.Find(String.Format("Spawner/Block {0}/GroundSpawns/Line {1}/GroundSpawn{2}", movingStartBlock - moveLength + 1, 1, 0));
				tilePopulation.MoveTrigger.Object = moveTrigger;
			}

			for (int block = movingStartBlock; block >= (movingStartBlock - moveLength); block--)
			{
				for (int boxcount = 0; boxcount < 3; boxcount++)
				{
					tilePopulation.Blocks[block][boxes][movingLineStart, boxcount].Alive = false;
				}

				tilePopulation.Blocks[block][spawnobstacles][0, movingLineStart].Alive = false;
				tilePopulation.Blocks[block][airspawns][0, movingLineStart].Alive = false;

				for (int groundspawncount = 0; groundspawncount < 5; groundspawncount++)
				{
					tilePopulation.Blocks[block][jumpoverobstacles][movingLineStart, groundspawncount].Alive = false;
				}

				for (int boxcoincount = 0; boxcoincount < tilePopulation.Blocks[block][boxcoins].GetLength(1); boxcoincount++)
				{
					if (tilePopulation.Blocks[block][boxcoins][movingLineStart, boxcoincount] != null)
					{
						tilePopulation.Blocks[block][boxcoins][movingLineStart, boxcoincount].Alive = false;
					}
				}
			}
		}

		private void FillBigObstacles(TilePopulation tilePopulation)
		{
			for (int e = 0; e < 5; e++)
			{
				roadbarier = 0;
				for (int i = 0; i < 3; i++)
				{
					GameObject obstacle = tilePopulation.Blocks[e][spawnobstacles][0, i].Object;
					if (tilePopulation.Blocks[e][spawnobstacles][0, i].Alive)
					{
						if (obstacle == cone | obstacle == bin | obstacle == container | obstacle == pizzaTruck)
						{
							roadbarier++;
						}
					}
				}

				if (roadbarier == 3)
				{
					tilePopulation.Blocks[e][spawnobstacles][0, 0].Alive = false;
					tilePopulation.Blocks[e][spawnobstacles][0, 1].Object = roadBarrier;
					tilePopulation.Blocks[e][spawnobstacles][0, 2].Alive = false;
				}
				
			}
		}

		private void Spawn(TilePopulation tilePopulation)
		{
			for (int blocknumber = 0; blocknumber < 5; blocknumber++)
			{
				var spawns = tilePopulation.Blocks[blocknumber][spawnobstacles].Cast<SpawnObject>()
						.Concat(tilePopulation.Blocks[blocknumber][groundspawns].Cast<SpawnObject>())
						.Concat(tilePopulation.Blocks[blocknumber][airspawns].Cast<SpawnObject>())
						.Concat(tilePopulation.Blocks[blocknumber][jumpoverobstacles].Cast<SpawnObject>())
						.Concat(tilePopulation.Blocks[blocknumber][airspawns].Cast<SpawnObject>())
						.Concat(tilePopulation.Blocks[blocknumber][boxcoins].Cast<SpawnObject>())
						.Concat(tilePopulation.Blocks[blocknumber][boxes].Cast<SpawnObject>());

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
			}

			if (tilePopulation.MoveTrigger.Alive && tilePopulation.MoveTrigger.Object != null && tilePopulation.MoveTrigger.Location != null)
			{
				Instantiate(
						tilePopulation.MoveTrigger.Object,
						tilePopulation.MoveTrigger.Location.position,
						tilePopulation.MoveTrigger.Location.rotation,
						tilePopulation.Tile.transform.Find("Spawner"));
			}

			const string defaultName = "New Game Object";
			foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject)).Where(x => x.name == defaultName))
			{
				Destroy(gameObject);
			}
		}
	}
}
