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
			string data = null;

			if (data != null)
			{
				animator = GetComponent<Animator>();
				r = new Replay(data);
				orientationQueue = r.GetOrientationQueue();
				orientation = orientationQueue.Dequeue();
				InvokeRepeating("UpdateOrientation", 0, REPLAY_INTERVAL);

				animator.SetFloat("Speed", 20);
				iTween.MoveTo(gameObject, iTween.Hash("path", r.Path, "time", r.Count * REPLAY_INTERVAL, "easetype", iTween.EaseType.linear));
				StartCoroutine(CoroutineHelper.Repeat(
					REPLAY_INTERVAL,
					() =>
					{
						Orientation newOrientation = orientationQueue.Dequeue();
						if (orientation != newOrientation)
						{
							iTween.RotateTo(gameObject, new Vector3(0, (int)newOrientation * 90, 0), 0.5f);
							orientation = newOrientation;
						}
					},
					() => orientationQueue.Count > 0,
					() =>
					{
						animator.SetFloat("Speed", 0);
						animator.Play("Wary");
						iTween.MoveTo(gameObject, gameObject.transform.position.CreateNew(y: 0.0f), 0.5f);
					}));
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}
