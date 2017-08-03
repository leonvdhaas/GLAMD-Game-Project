using UnityEngine;

namespace Assets.Scripts.Extensions
{
	public static class Vector3Extensions
	{
		public static Vector3 CreateNew(this Vector3 vector, float? x = null, float? y = null, float? z = null)
		{
			return new Vector3
			{
				x = x.HasValue ? x.Value : vector.x,
				y = y.HasValue ? y.Value : vector.y,
				z = z.HasValue ? z.Value : vector.z
			};
		}

		public static Vector3 Multiply(this Vector3 value, Vector3 other)
		{
			return new Vector3
			{
				x = value.x * other.x,
				y = value.y * other.y,
				z = value.z * other.z
			};
		}
	}
}
