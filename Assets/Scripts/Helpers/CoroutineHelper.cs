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
				yield return new WaitForSeconds(interval);
				action();
			}

			finish.NullSafeOperation(x => x.Invoke());
		}

		public static IEnumerator WaitUntil(Func<bool> predicate, Action action)
		{
			yield return new WaitUntil(predicate);
			action();
		}

		public static IEnumerator RepeatFor(float interval, int amount, Action<int> action)
		{
			return RepeatFor(interval, amount, action, null);
		}

		public static IEnumerator RepeatFor(float interval, int amount, Action<int> action, Action finish)
		{
			for (int i = 0; i < amount; i++)
			{
				yield return new WaitForSeconds(interval);
				action(i);
			}

			finish.NullSafeOperation(x => x.Invoke());
		}
	}
}
