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

				ReadRotatingQueue();
				StartCoroutine(CoroutineHelper.Delay(4, () => animator.SetFloat("Speed", 0.75f)));
				StartCoroutine(CoroutineHelper.Delay(
					0.0375f,
					() => iTween.MoveTo(
						gameObject,
						iTween.Hash(
							"path",
							replay.Path,
							"time", replay.Count * REPLAY_INTERVAL,
							"easetype",
							iTween.EaseType.linear))));
			}
			else
			{
				Destroy(gameObject);
			}
		}

		private void ReadRotatingQueue()
		{
			StartCoroutine(CoroutineHelper.Repeat(
				REPLAY_INTERVAL,
				() =>
				{
					Orientation newOrientation = orientationQueue.Dequeue();
					if (orientation != newOrientation)
					{
						iTween.RotateTo(gameObject, new Vector3(0, (int)newOrientation * 90, 0), 0);
						orientation = newOrientation;
					}
				},
				() => orientationQueue.Count > 0,
				() =>
				{
					animator.SetFloat("Speed", 0);
					iTween.MoveTo(gameObject, gameObject.transform.position.CreateNew(y: -0.5f), 0.5f);
					StartCoroutine(CoroutineHelper.Delay(10, () => Destroy(gameObject)));
				}));
		}
	}
}
