using Assets.Scripts.Enumerations;
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

		private AudioSource audioSource;
		private AudioClip audioClip;

		public static SoundManager Instance { get; private set; }

		public void PlaySound(Sound sound)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
			switch (sound)
			{
				case Sound.Coin:
					audioClip = (AudioClip)Resources.Load("SFX/coin");
					break;
				case Sound.Slowmotion:
					audioClip = (AudioClip)Resources.Load("SFX/slowmotion");
					break;
				case Sound.Inhaler:
					audioClip = (AudioClip)Resources.Load("SFX/inhaler");
					break;
				case Sound.Thud:
					audioClip = (AudioClip)Resources.Load("SFX/thud");
					break;
				case Sound.BoxInvincibleBreak:
					audioClip = (AudioClip)Resources.Load("SFX/box_break");
					break;
				default:
					break;
			}
			audioSource.clip = audioClip;
			audioSource.Play();
		}
	}
}
