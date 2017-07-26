using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
	public class ReplayGhostController
		: MonoBehaviour
	{
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
				InvokeRepeating("UpdateOrientation", 0, 0.2f);
				animator.SetFloat("Speed", 10);
				iTween.MoveTo(gameObject, iTween.Hash("path", r.Path, "time", r.Count * 0.2f, "easetype", iTween.EaseType.linear));
			}
			else
				gameObject.SetActive(false);
		}

		void UpdateOrientation()
		{
			if (orientationQueue.Count > 0)
			{
				Orientation newOrientation = orientationQueue.Dequeue();
				if (orientation != newOrientation)
				{
					iTween.RotateTo(gameObject, new Vector3(0, (int)newOrientation * 90, 0), 0.5f);
					orientation = newOrientation;
				}
			}
			else
			{
				CancelInvoke("UpdateOrientation");
				animator.SetFloat("Speed", 0);
				animator.Play("Wary");
				iTween.MoveTo(gameObject, gameObject.transform.position.CreateNew(y: 0.0f), 0.5f);
			}
		}
	}
}
