using UnityEngine;
using System.Collections;
using Assets.Scripts.Gameplay.Models;

namespace Assets.Scripts.Gameplay.Models
{
    public class Spawn
    {
        public int P, C, N;
        public void Fill(int p, int c, int n)
        {
            P = p;
            C = c;
            N = n;
        }
    }
}
