using Assets.Scripts.Extensions;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
	public static class CoroutineHelper
	{
		public static IEnumerator Delay(float delay, Action action)
		{
			yield return new WaitForSeconds(delay);
			action();
		}

		public static IEnumerator Repeat(float interval, Action action)
		{
			return Repeat(interval, action, () => true);
		}

		public static IEnumerator Repeat(float interval, Action action, Func<bool> predicate)
		{
			return Repeat(interval, action, predicate, null);
		}

		public static IEnumerator Repeat(float interval, Action action, Func<bool> predicate, Action finish)
		{
			while (predicate())
			{
				action();
				yield return new WaitForSeconds(interval);
			}

			finish.NullSafeOperation(x => x.Invoke());
		}

		public static IEnumerator WaitUntil(Func<bool> predicate, Action action)
		{
			yield return new WaitUntil(predicate);
			action();
		}

		public static IEnumerator For(float interval, int start, int limit, Action<int> action)
		{
			return RepeatFor(interval, start, limit, action, null);
		}

		public static IEnumerator RepeatFor(float interval, int start, int limit, Action<int> action, Action finish)
		{
			for (int i = start; i < limit; i++)
			{
				action(i);
				yield return new WaitForSeconds(interval);
			}

			finish.NullSafeOperation(x => x.Invoke());
		}

		public static IEnumerator ActionQueue(float interval, params Action[] actions)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i]();
				yield return new WaitForSeconds(interval);
			}
		}
	}
}
