using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.Triggers
{
	public class CornerEndTileTrigger
		: MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			var player = other.GetComponent<PlayerController>();
			if (player != null) {
				player.TakeFailedCorner();
				GetComponent<Collider>().enabled = false;
			}
		}
	}
}
