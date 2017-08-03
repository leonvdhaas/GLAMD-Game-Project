using UnityEngine;
using System.Collections;
using Assets.Scripts.Gameplay.Models;


namespace Assets.Scripts.Gameplay
{
    public class Generator
    {
        public SpawnObstacle SO = new SpawnObstacle();
        public Spawn S = new Spawn();
        public AirSpawnObstacle ASO = new AirSpawnObstacle();
        public void Fill (int o, int p, int c, int n)
        {
            SO.Fill(o, p, c, n);
            S.Fill(p, c, (100 - (p + c)));
            ASO.Fill(c, (100 - c));
        }
    }
}
