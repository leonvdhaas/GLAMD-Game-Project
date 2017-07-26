using UnityEngine;

namespace Assets.Scripts
{
	public class Loader
		: MonoBehaviour
	{
		[SerializeField]
		private GameObject[] loadObjects;

		private void Awake()
		{
			foreach (var loadObj in loadObjects)
			{
				Instantiate(loadObj);
			}
		}
	}
}
