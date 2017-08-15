using Assets.Scripts.Extensions;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
	public static class CoroutineHelper
	{
		public delegate void Afterthought<T>(ref T arg1);

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

		public static IEnumerator RepeatFor(float interval, int amountOfTimes, Action action)
		{
			return RepeatFor(interval, amountOfTimes, action, null);
		}

		public static IEnumerator RepeatFor(float interval, int amountOfTimes, Action action, Action finish)
		{
			for (int i = 0; i < amountOfTimes; i++)
			{
				action();
				yield return new WaitForSeconds(interval);
			}

			finish.NullSafeOperation(x => x.Invoke());
		}

		public static IEnumerator For<T>(
			float interval,
			Func<T> initialization,
			Func<T, bool> predicate,
			Afterthought<T> afterthought,
			Action<T> action)
		{
			return For(interval, initialization, predicate, afterthought, action, null);
		}

		public static IEnumerator For<T>(
			float interval,
			Func<T> initialization,
			Func<T, bool> predicate,
			Afterthought<T> afterthought,
			Action<T> action,
			Action finish)
		{
			for (var i = initialization(); predicate(i); afterthought(ref i))
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
