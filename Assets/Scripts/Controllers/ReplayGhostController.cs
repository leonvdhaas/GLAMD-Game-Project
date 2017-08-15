using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;
using Assets.Scripts.Helpers;
using Assets.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
	public class ReplayGhostController
		: MonoBehaviour
	{
		public const float REPLAY_INTERVAL = 0.2f;

		private Orientation orientation;
		private Queue<Orientation> orientationQueue;
		private Animator animator;

		void Start()
		{
			var replay = GameManager.Instance.CurrentGame.Replay;
			if (GameManager.Instance.CurrentGame.GameType == GameType.MultiplayerChallenge)
			{
				animator = GetComponent<Animator>();
				orientationQueue = replay.GetOrientationQueue();
				orientation = orientationQueue.Dequeue();

				StartCoroutine(CoroutineHelper.Delay(2.5f, () => animator.SetFloat("Speed", 15)));
				StartCoroutine(CoroutineHelper.Repeat(
					REPLAY_INTERVAL,
					() =>
					{
						Orientation newOrientation = orientationQueue.Dequeue();
						if (orientation != newOrientation)
						{
							iTween.RotateTo(gameObject, new Vector3(0, (int)newOrientation * 90, 0), 0.1f);
							orientation = newOrientation;
						}
					},
					() => orientationQueue.Count > 0,
					() =>
					{
						animator.SetFloat("Speed", 0);
						iTween.MoveTo(gameObject, gameObject.transform.position.CreateNew(y: -0.5f), 0.5f);
					}));
				iTween.MoveTo(gameObject, iTween.Hash("path", replay.Path, "time", replay.Count * REPLAY_INTERVAL, "easetype", iTween.EaseType.linear));
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}
