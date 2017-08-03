using UnityEngine;
using System.Collections;
using Assets.Scripts.Gameplay;


namespace Assets.Scripts.Gameplay
{
    public class SpawnObstacle
    {
        public int O, P, C, N;
        public void Fill(int o, int p, int c, int n)
        {
            O = o;
            P = p;
            C = c;
            N = n;
        }
    }
}