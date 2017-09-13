using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Enumerations;

namespace Assets.Scripts.Models
{
	[Serializable]
	public class Particle
	{
		[SerializeField]
		private ParticleSystem _particleSystem;

		public ParticleSystem ParticleSystem
		{
			get { return _particleSystem; }
			set { _particleSystem = value; }
		}

		[SerializeField]
		private ParticleType _particleType;

		public ParticleType ParticleType
		{
			get { return _particleType; }
			set { _particleType = value; }
		}
	}
}
