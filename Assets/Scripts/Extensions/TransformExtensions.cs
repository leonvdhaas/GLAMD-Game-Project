using System;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
	public static class TransformExtensions
	{
		public static Transform GetParent(this Transform transform, int depth = 1)
		{
			for (int i = 0; i < depth; i++)
			{
				if (transform.parent == null)
				{
					throw new InvalidOperationException("Transform has no parent.");
				}

				transform = transform.parent;
			}

			return transform;
		}
	}
}
