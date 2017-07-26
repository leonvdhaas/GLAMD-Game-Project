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

			finish();
		}
	}
}
