using Assets.Scripts.Enumerations;
using System;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
	public static class OrientationExtensions
	{
		public static Orientation GetLeftOrientation(this Orientation orientation)
		{
			switch (orientation)
			{
				case Orientation.North: return Orientation.West;
				case Orientation.East: return Orientation.North;
				case Orientation.South: return Orientation.East;
				case Orientation.West: return Orientation.South;
				default: throw new ArgumentException("Invalid orientation provided.");
			}
		}

		public static Orientation GetRightOrientation(this Orientation orientation)
		{
			switch (orientation)
			{
				case Orientation.North: return Orientation.East;
				case Orientation.East: return Orientation.South;
				case Orientation.South: return Orientation.West;
				case Orientation.West: return Orientation.North;
				default: throw new ArgumentException("Invalid orientation provided.");
			}
		}

		public static Vector3 GetDirectionVector3(this Orientation orientation)
		{
			switch (orientation)
			{
				case Orientation.North: return Vector3.forward;
				case Orientation.East: return Vector3.right;
				case Orientation.South: return Vector3.back;
				case Orientation.West: return Vector3.left;
				default: throw new ArgumentException("Invalid orientation provided.");
			}
		}

        public static Orientation GetOppositeOrientation(this Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.North: return Orientation.South;
                case Orientation.East: return Orientation.West;
                case Orientation.South: return Orientation.North;
                case Orientation.West: return Orientation.East;
                default: throw new ArgumentException("Invalid orientation provided.");
            }
        }
    }
}
