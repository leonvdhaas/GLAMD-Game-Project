﻿using System;

namespace Assets.Scripts.Helpers
{
    abstract class GlobalRandom
    {
        public static new bool Equals(object obj)
        {
            return rng.Equals(obj);
        }

        public static new int GetHashCode()
        {
            return rng.GetHashCode();
        }
        public static new Type GetType()
        {
            return rng.GetType();
        }

        public static new string ToString()
        {
            return rng.ToString();
        }

        public static int Next()
        {
            return rng.Next();
        }

        public static int Next(int maxValue)
        {
            return rng.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return rng.Next(minValue, maxValue);
        }

        public static void NextBytes(byte[] buffer)
        {
            rng.NextBytes(buffer);
        }

        public static double NextDouble()
        {
            return rng.NextDouble();
        }

        private static Random rng = new Random(seed = DateTime.Now.Millisecond);
        private static int seed;
        public static int Seed
        {
            get
            {
                return seed;
            }
            set
            {
                rng = new Random(seed = value);
            }
        }
    }
}