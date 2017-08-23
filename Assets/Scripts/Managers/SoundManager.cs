using Assets.Scripts.Enumerations;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
	public class SoundManager
		: MonoBehaviour
	{
		[SerializeField]
		private new Audio[] soundEffects;

		private AudioSource audioSource;

		// Use this for initialization
		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);

				audioSource = GetComponent<AudioSource>();

				MusicVolume = 1.0f;
				SoundEffectVolume = 1.0f;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public static SoundManager Instance { get; private set; }

		public float SoundEffectVolume { get; private set; }

		public float MusicVolume { get; private set; }

		public void PlaySoundEffect(Sound sound)
		{
			audioSource.PlayOneShot(soundEffects.Single(x => x.Sound == sound).Clip, SoundEffectVolume);
		}
	}
}
