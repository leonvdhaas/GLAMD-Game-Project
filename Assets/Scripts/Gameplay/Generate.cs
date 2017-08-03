using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Text;
using Assets.Scripts.Gameplay.Models;
using Assets.Scripts.Helpers;

namespace Assets.Scripts.Gameplay
{
	public class Generate : MonoBehaviour
	{
		// Blocks of gameplay
		public List<SpawnObject[,]> Box1 = new List<SpawnObject[,]>();
		public List<SpawnObject[,]> Box2 = new List<SpawnObject[,]>();
		public List<SpawnObject[,]> Box3 = new List<SpawnObject[,]>();
		public List<SpawnObject[,]> Box4 = new List<SpawnObject[,]>();
		public List<SpawnObject[,]> Box5 = new List<SpawnObject[,]>();

		// SpawnPositions
		public SpawnObject[,] SpawnObstacles = new SpawnObject[1, 3]; 
		public SpawnObject[,] GroundSpawns = new SpawnObject[3, 5]; 
		public SpawnObject[,] JumpOverObstacles = new SpawnObject[3, 5];
		public SpawnObject[,] AirSpawns = new SpawnObject[1, 3]; 

		// Obstacles
		public GameObject CartoonCar;
		public GameObject Jeep;
		public GameObject Box;
		public GameObject Hotdogtruck;
		public GameObject Pizzatruck;

		// Powerups
		public GameObject DoubleCoins;
		public GameObject Inhaler;

		// Coins
		public GameObject Coin;
		public GameObject Diamond;

		// Chance Generator
		public Generator Chances = new Generator();

		// Dependencies en chancevalues voor het bijhouden van variaties aan gameplay en wiskunde (uitschakelen en kansberekenen van objecten en gameplay)
		private bool bigobstacle = false;
		private int powerupcount = 0;
		private int coinbowcount = 0;
		private int truckcount = 0;
		private int bigtype;
		private int spawncounter = 0;
		private int listcounter = 0;
		private int o, p, c, n;
		private object[] obstacletype = new object[5];

		void Start()
		{
			
		}

		public int GenerateChance()
		{
			int c = GlobalRandom.Next(0, 100);
			return c;
		}

		void OnEnable()
		{
			Chances.Fill(45, 20, 10, 15);
			Assign();
		}

		GameObject PowerupChance()
		{
			GameObject pc = new GameObject();
			int powerupchance = GlobalRandom.Next(1, 2);
			switch (powerupchance)
			{
				case 1:
					pc = Inhaler;
					break;
				case 2:
					pc = DoubleCoins;
					break;
			}
			return pc;
		}

		GameObject CoinChance()
		{
			GameObject c = new GameObject();
			int coinchance = GlobalRandom.Next(0, 100);
			if (coinchance < 5) // Het diamandje heeft nu een kans van 5 procent als er een muntje word gespawnt, 
			{
				c = Diamond;
			}
			else
			{
				c = Coin;
			}
			return c;
		}

		GameObject ObstacleChance()
		{
			GameObject o = new GameObject();
			int obstaclechance = GlobalRandom.Next(0, 100);
			if (obstaclechance < 50)
			{
				o = Box;
			}
			else if (obstaclechance < 80)
			{
				o = CartoonCar;
			}
			else
			{
				o = Pizzatruck;
			}
			return o;
		}

		void Assign()
		{
			// assign each spawnobject to corresponding transform
			for (int o = 1; o <= 5; o++)
			{
				for (int i = 0; i < 3; i++)
				{
					SpawnObstacles[0, i] = new SpawnObject();
					SpawnObstacles[0, i].loc = transform.Find("/Spawner/Block " + o.ToString() +"/Obstacles/SpawnObstacle" + (i + 1).ToString());
					AirSpawns[0, i] = new SpawnObject();
					AirSpawns[0, i].loc = transform.Find("/Spawner/Block " + o.ToString() + "/AirSpawns/AirSpawn" + (i + 1).ToString());

					for (int e = 0; e < 5; e++)
					{
						GroundSpawns[i, e] = new SpawnObject();
						GroundSpawns[i, e].loc = transform.Find("/Spawner/Block " + o.ToString() + "/GroundSpawns/Line " + (i + 1).ToString() + "/GroundSpawn" + (e + 1).ToString());
						JumpOverObstacles[i, e] = new SpawnObject();
						JumpOverObstacles[i, e].loc = transform.Find("/Spawner/Block " + o.ToString() + "/JumpOverObstacle/Line " + (i + 1).ToString() + "/AirObstacleSpawn" + (e + 1).ToString());
					}
				}
				Fill();
			}
		}

		void Fill()
		{
			foreach (SpawnObject o in SpawnObstacles)
			{
				int i = GenerateChance();
				if (i < Chances.SO.O)
				{
					o.obj = Obstacle(o);
				}
				else if (i < Chances.SO.O + Chances.SO.P)
				{
					o.obj = PowerUp(o);
				}
				else if (i < Chances.SO.O + Chances.SO.P + Chances.SO.C)
				{
					o.obj = Coins(o);
				}
				else
				{
					o.alive = false;
				}
			}
			foreach (SpawnObject o in GroundSpawns)
			{
				Pickup(o);
			}
			foreach (SpawnObject o in AirSpawns)
			{
				Pickup(o);
			}
			for (int i = 0; i < 3; i++)
			{
				if (SpawnObstacles[0, i].obj == Box)
				{
					for (int a = 0; a < 5; a++)
					{
						JumpOverObstacles[i, a].obj = Coins(JumpOverObstacles[i, a]);
					}
					AirSpawns[0, i].alive = false;
				}
				else
				{
					for (int a = 0; a < 5; a++)
					{
						JumpOverObstacles[i, a].alive = false;
					}
				}
			}
			Spawn();
		}

		void Pickup(SpawnObject p)
		{
			int i = GenerateChance();
			if (i < Chances.S.P)
			{
				p.obj = PowerUp(p);
			}
			else if (i < Chances.S.P + Chances.S.C)
			{
				p.obj = Coins(p);
			}
			else
			{
				p.alive = false;
			}
		}

		void Clean()
		{
			foreach (SpawnObject obstacle in JumpOverObstacles)
			{
				int i = GenerateChance();
				if (i < Chances.ASO.C)
				{
					obstacle.obj = Coins(obstacle);
				}
				else
				{
					obstacle.alive = false;
				}

			}
		}

		GameObject PowerUp(SpawnObject spawn)
		{
			GameObject powerup = new GameObject();

			if (powerupcount < 1)// hoeveelheid powerups per straight tile
			{
				powerup = PowerupChance();
				powerupcount++;
			}
			else
			{
				spawn.alive = false;
			}

			return powerup;
		}

		GameObject Coins(SpawnObject spawn)
		{
			GameObject coin = new GameObject();
			coin = CoinChance();
			return coin;
		}

		GameObject Obstacle(SpawnObject spawn)
		{
			GameObject obstakel = new GameObject();
			obstakel = ObstacleChance();
			return obstakel;
		}

		void Spawn()
		{
			int counter = 0;
			foreach (SpawnObject spawn in SpawnObstacles)
			{
				if (spawn != null && spawn.alive && spawn.obj != null && spawn.loc != null && spawn.loc.position != null && spawn.loc.rotation != null)
				{
					GameObject spawned = Instantiate(spawn.obj, spawn.loc.position, spawn.loc.rotation) as GameObject;
					spawned.transform.parent = GameObject.Find(transform.name + "Spawner/").transform;
					counter++;
				}
			}
			foreach (SpawnObject spawn in GroundSpawns)
			{
				if (spawn != null && spawn.alive && spawn.obj != null && spawn.loc != null && spawn.loc.position != null && spawn.loc.rotation != null)
				{
					GameObject spawned = Instantiate(spawn.obj, spawn.loc.position, spawn.loc.rotation) as GameObject;
					spawned.transform.parent = GameObject.Find(transform.name + "Spawner/").transform;
					counter++;
				}
			}
			foreach (SpawnObject spawn in AirSpawns)
			{
				if (spawn != null && spawn.alive && spawn.obj != null && spawn.loc != null && spawn.loc.position != null && spawn.loc.rotation != null)
				{
					GameObject spawned = Instantiate(spawn.obj, spawn.loc.position, spawn.loc.rotation) as GameObject;
					spawned.transform.parent = GameObject.Find(transform.name + "Spawner/").transform;
					counter++;
				}
			}
			foreach (SpawnObject spawn in JumpOverObstacles)
			{
				if (spawn != null && spawn.alive && spawn.obj != null && spawn.loc != null && spawn.loc.position != null && spawn.loc.rotation != null)
				{
					GameObject spawned = Instantiate(spawn.obj, spawn.loc.position, spawn.loc.rotation) as GameObject;
					spawned.transform.parent = GameObject.Find(transform.name + "Spawner/").transform;
					counter++;
				}
			}
			foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
			{
				if (go.name == "New Game Object")
				{
					Destroy(go);
				}
			}
		}
	}
}
