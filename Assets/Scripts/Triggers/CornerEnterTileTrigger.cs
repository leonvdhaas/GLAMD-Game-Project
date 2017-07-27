using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Models.Triggers
{
	public class CornerEnterTileTrigger
		: MonoBehaviour
	{
		private const float TRIGGER_OFFSET = 2.0f;

		private void OnTriggerEnter(Collider other)
		{
			var player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				if (GetComponentInParent<LeftCornerTile>() != null)
				{
					player.IsOnLeftCorner = true;
				}
				else
				{
					player.IsOnRightCorner = true;
				}

				var turningPositions = new LanePosition[Tile.LANES];
				for (int i = 0; i < Tile.LANES; i++)
				{
					turningPositions[i] = new LanePosition
					{
						Lane = (Lane)(player.IsOnLeftCorner ? i : 2 - i),
						Position = transform.position + player.Orientation.GetDirectionVector3() * (TRIGGER_OFFSET + i * Tile.LANE_DISTANCE)
					};
				}

				player.TurningPositions = turningPositions;
				GetComponent<Collider>().enabled = false;
			}
		}
	}
}
