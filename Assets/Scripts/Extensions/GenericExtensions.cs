using Assets.Scripts.Models;
using System;

namespace Assets.Scripts.Extensions
{
	public static class GenericExtensions
	{
		public static WeightedItem<T> ToWeightedItem<T>(this T value, byte weight)
		{
			return new WeightedItem<T>(value, weight);
		}

		public static void NullSafeOperation<T>(this T value, Action<T> operation)
			where T
			:  class
		{
			if (operation == null)
			{
				throw new ArgumentNullException("operation");
			}

			if (value != null)
			{
				operation.Invoke(value);
			}
		}
	}
}
