using Assets.Scripts.Models;

namespace Assets.Scripts.Extensions
{
	public static class ObjectExtensions
	{
		public static WeightedItem<T> ToWeightedItem<T>(this T value, byte weight)
		{
			return new WeightedItem<T>(value, weight);
		}
	}
}
