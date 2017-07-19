using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Models
{
	public struct WeightedItem<T>
		: IWeighted
	{
		public WeightedItem(T value, byte weight)
		{
			Value = value;
			Weight = weight;
		}

		public T Value { get; private set; }

		public byte Weight { get; private set; }
	}
}
