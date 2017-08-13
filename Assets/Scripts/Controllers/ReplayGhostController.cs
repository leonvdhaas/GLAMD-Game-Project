using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
	public class ReplayGhostController
		: MonoBehaviour
	{
		public const float REPLAY_INTERVAL = 0.2f;

		private Replay r;
		private Orientation orientation;
		private Queue<Orientation> orientationQueue;
		private Animator animator;

		void Start()
		{
			string data = "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";

			if (data != null)
			{
				animator = GetComponent<Animator>();
				r = new Replay(data);
				orientationQueue = r.GetOrientationQueue();
				orientation = orientationQueue.Dequeue();

				StartCoroutine(CoroutineHelper.Delay(2.5f, () => animator.SetFloat("Speed", 10)));
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
				iTween.MoveTo(gameObject, iTween.Hash("path", r.Path, "time", r.Count * REPLAY_INTERVAL, "easetype", iTween.EaseType.linear));
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}
