using Assets.Scripts.Enumerations;
using System;
using UnityEngine;

namespace Assets.Scripts.Models
{
	public struct ReplayDataPoint
	{
		public int Index { get; set; }
		public Vector3 Position { get; set; }
		public Orientation Orientation { get; set; }

		public ReplayDataPoint(string info)
		{
			string[] data = info.Split(',');
			Index = Convert.ToInt32(data[0]);
			Position = new Vector3(
				x: Convert.ToSingle(data[1]),
				y: Convert.ToSingle(data[2]),
				z: Convert.ToSingle(data[3]));
			Orientation = (Orientation)Convert.ToByte(data[4]);
		}

		public ReplayDataPoint(int index, Vector3 pos, Orientation orientation)
		{
			Index = index;
			Position = pos;
			Orientation = orientation;
		}

		public override string ToString()
		{
			return String.Format("{0},{1:0.0},{2:0.0},{3:0.0},{4}", Index, Position.x, Position.y, Position.z, (int)Orientation);
		}
	}
}
