using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Models.Triggers
{
	class RightCornerTrigger
		: MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<PlayerController>() != null) {
				TileManager.Instance.AddRandomTile();
			}
		}
	}
}
