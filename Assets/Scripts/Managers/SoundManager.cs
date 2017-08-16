using Assets.Scripts.Enumerations;
using Assets.Scripts.Helpers;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
	public class SoundManager
		: MonoBehaviour
	{
		// Use this for initialization
		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		[SerializeField]
		private AudioClip coin;

		[SerializeField]
		private AudioClip slowmotion;

		[SerializeField]
		private AudioClip inhaler;

		[SerializeField]
		private AudioClip thud;

		[SerializeField]
		private AudioClip boxBreak;

		[SerializeField]
		private AudioClip heart;

		[SerializeField]
		private AudioClip coinDoubler;

		[SerializeField]
		private AudioClip diamond;

		[SerializeField]
		private AudioClip inhalerActivate;

		public static SoundManager Instance { get; private set; }

		public void PlaySound(Sound sound)
		{
			var audioSource = gameObject.AddComponent<AudioSource>();
			switch (sound)
			{
				case Sound.Coin:
					audioSource.clip = coin;
					break;
				case Sound.Slowmotion:
					audioSource.clip = slowmotion;
					break;
				case Sound.Inhaler:
					audioSource.clip = inhaler;
					break;
				case Sound.Thud:
					audioSource.clip = thud;
					break;
				case Sound.BoxInvincibleBreak:
					audioSource.clip = boxBreak;
					break;
				case Sound.Diamond:
					audioSource.clip = diamond;
					break;
				case Sound.CoinDoubler:
					audioSource.clip = coinDoubler;
					break;
				case Sound.Heart:
					audioSource.clip = heart;
					break;
				case Sound.InhalerActivate:
					audioSource.clip = inhalerActivate;
					break;
				default:
					throw new InvalidOperationException("Invalid sound type.");
			}

			audioSource.Play();
			StartCoroutine(CoroutineHelper.Delay(audioSource.clip.length, () => Destroy(audioSource)));
		}
	}
}
