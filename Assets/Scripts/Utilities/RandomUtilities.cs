using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Utilities
{
	public static class RandomUtilities
	{
		public const double PERCENT = 1.0;
		public const double HUNDRED_PERCENT = 100 * PERCENT;
		private static readonly Random RNG = new Random();

		public static double FractureAsPercentage(int x, int y)
		{
			return HUNDRED_PERCENT * x / y;
		}

		public static bool PercentageChance(double percentage)
		{
			if (percentage <= 0) {
				return false;
			}
			else if (percentage >= 100) {
				return true;
			}

			return percentage >= RNG.NextDouble() * HUNDRED_PERCENT;
		}

		public static T Pick<T>(params T[] collection)
		{
			return collection.Pick();
		}

		public static T Pick<T>(this IEnumerable<T> collection)
		{
			return collection.ElementAt(RNG.Next(collection.Count()));
		}

		public static T WeightedPick<T>(params WeightedItem<T>[] weightedCollection)
		{
			return weightedCollection.WeightedPick();
		}

		public static T WeightedPick<T>(this IEnumerable<WeightedItem<T>> weightedCollection)
		{
			int totalWeight = weightedCollection.Sum(x => x.Weight);
			int randomNumber = RNG.Next(totalWeight);
			foreach (var item in weightedCollection) {
				if (randomNumber < item.Weight) {
					return item.Value;
				}

				randomNumber -= item.Weight;
			}

			return default(T);
		}

		public static void PerformAction(params Action[] actions)
		{
			actions.PerformAction();
		}

		public static void PerformAction(this IEnumerable<Action> actions)
		{
			actions.Pick().Invoke();
		}

		public static TResult PerformFunction<TResult>(params Func<TResult>[] functions)
		{
			return functions.PerformFunction();
		}

		public static TResult PerformFunction<TResult>(this IEnumerable<Func<TResult>> functions)
		{
			return functions.Pick().Invoke();
		}
	}
}
