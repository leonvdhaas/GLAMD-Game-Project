using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Models.Codes
{
	public class AlternativeMusic
		: Code
	{
		[SerializeField]
		private AudioClip alternativeClip;

		protected override void Activate()
		{
			SoundManager.Instance.SetBackgroundMusic(alternativeClip);
		}
	}
}
