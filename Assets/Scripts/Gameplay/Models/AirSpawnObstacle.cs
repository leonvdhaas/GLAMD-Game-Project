using UnityEngine;
using System.Collections;
using Assets.Scripts.Gameplay.Models;

namespace Assets.Scripts.Gameplay.Models
{
    public class AirSpawnObstacle
    {
        public int C, N;
        public void Fill(int c, int n)
        {
            C = c;
            N = n;
        }
    }
}
