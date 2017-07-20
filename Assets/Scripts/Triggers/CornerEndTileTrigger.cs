using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;
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
			}
		}
	}
}
