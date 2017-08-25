using Assets.Scripts.Enumerations;
using Assets.Scripts.Models;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
	public class SoundManager
		: MonoBehaviour
	{
		[SerializeField]
		private Audio[] soundEffects;

		private AudioSource sfxAudioSource;
		private AudioSource musicAudioSource;

		// Use this for initialization
		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);

				musicAudioSource = GetComponent<AudioSource>();
				sfxAudioSource = gameObject.AddComponent<AudioSource>();

				SoundEffectVolume = 1;
				MusicVolume = 1;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public float SoundEffectVolume { get; set; }

		public float MusicVolume
		{
			get { return musicAudioSource.volume; }
			set { musicAudioSource.volume = value; }
		}

		public static SoundManager Instance { get; set; }

		public void PlaySoundEffect(Sound sound)
		{
			sfxAudioSource.PlayOneShot(soundEffects.Single(x => x.Sound == sound).Clip, SoundEffectVolume);
		}

		public void SetBackgroundMusic(AudioClip clip)
		{
			musicAudioSource.Stop();
			musicAudioSource.clip = clip;
			musicAudioSource.Play();
		}
	}
}
