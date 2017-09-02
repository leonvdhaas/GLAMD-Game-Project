using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.Triggers
{
	public class MoveTrigger
		: MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			var player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				var car = GetComponentInParent<CarController>();
				if (car != null)
				{
					// De-parent trigger, so the trigger doesn't move along with the car.
					transform.parent = null;
					car.StartDriving(player.Orientation.GetOppositeOrientation());
				}
			}
			else if (other.GetComponentInParent<CarController>())
			{
				Destroy(other.gameObject);
			}
		}
	}
}
