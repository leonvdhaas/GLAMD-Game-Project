using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Models
{
	public struct WeightedItem<T>
		: IWeighted
	{
		public WeightedItem(T value, ushort weight)
		{
			Value = value;
			Weight = weight;
		}

		public T Value { get; private set; }

		public ushort Weight { get; private set; }
	}
}
