using Assets.Scripts.Enumerations;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Extensions;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Replay
	{
		private List<ReplayDataPoint> dataPoints;

		public Replay()
		{
			dataPoints = new List<ReplayDataPoint>();
		}

		public Replay(string data)
		{
			dataPoints = data.Split('_').Select(info => new ReplayDataPoint(info)).OrderBy(x => x.Index).ToList();
		}

		public void Add(ReplayDataPoint info)
		{
			dataPoints.Add(info);
		}

		public Queue<Orientation> GetOrientationQueue()
		{
			return new Queue<Orientation>(dataPoints.OrderBy(x => x.Index).Select(x => x.Orientation));
		}

		public Vector3[] Path
		{
			get
			{
				return dataPoints.OrderBy(x => x.Index).Select(x => x.Position).ToArray();
			}
		}

		public int Count
		{
			get
			{
				return dataPoints.Count;
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Join("_", dataPoints.Select(x => x.ToString()).ToArray());
			return sb.ToString();
		}
	}
}
