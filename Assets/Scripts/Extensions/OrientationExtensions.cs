using Assets.Scripts.Enumerations;
using System;

namespace Assets.Scripts.Extensions
{
	public static class OrientationExtensions
	{
		public static Orientation GetLeftOrientation(this Orientation orientation)
		{
			switch (orientation) {
				case Orientation.North: return Orientation.West;
				case Orientation.East: return Orientation.North;
				case Orientation.South: return Orientation.East;
				case Orientation.West: return Orientation.South;
				default: throw new ArgumentException("Invalid orientation provided.");
			}
		}

		public static Orientation GetRightOrientation(this Orientation orientation)
		{
			switch (orientation) {
				case Orientation.North: return Orientation.East;
				case Orientation.East: return Orientation.South;
				case Orientation.South: return Orientation.West;
				case Orientation.West: return Orientation.North;
				default: throw new ArgumentException("Invalid orientation provided.");
			}
		}
	}
}
