using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Models.Triggers
{
	public class CornerEnterTileTrigger
		: MonoBehaviour
	{
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

				var turningPositions = new LanePosition[3];
				for (int i = 0; i < 3; i++)
				{
					turningPositions[i] = new LanePosition
					{
						Lane = (Lane)(player.IsOnLeftCorner ? i : 2 - i),
						Position = transform.position + player.Orientation.GetDirectionVector3() * (1.5f + i * 2)
					};
				}

				player.TurningPositions = turningPositions;
			}
		}
	}
}
