using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;
using Assets.Scripts.Models.Tiles;
using UnityEngine;

namespace Assets.Scripts.Models.Triggers
{
	public class TileEnterTrigger
		: MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<PlayerController>() != null)
			{
				GameManager.Instance.Player.CurrentTile = GetComponentInParent<Tile>();
				TileManager.Instance.AddRandomTile();
				GetComponent<Collider>().enabled = false;
			}
		}
	}
}
