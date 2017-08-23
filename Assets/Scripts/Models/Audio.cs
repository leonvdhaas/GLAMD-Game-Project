using Assets.Scripts.Enumerations;
using System;
using UnityEngine;

namespace Assets.Scripts.Models
{
	[Serializable]
	public class Audio
	{
		[SerializeField]
		private AudioClip _clip;

		public AudioClip Clip
		{
			get { return _clip; }
			set { _clip = value; }
		}

		[SerializeField]
		private Sound _sound;

		public Sound Sound
		{
			get { return _sound; }
			set { _sound = value; }
		}
	}
}
