using UnityEngine;

namespace Assets.Scripts.Models
{
	public class SpawnObject
	{
		public SpawnObject()
		{
			Alive = true;
			Move = false;
		}

		public Transform Location { get; set; }

		public GameObject Object { get; set; }

		public bool Alive { get; set; }

		public bool Move { get; set; }
	}
}
