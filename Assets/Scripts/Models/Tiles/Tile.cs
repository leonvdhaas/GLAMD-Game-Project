using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;
using UnityEngine;
using Assets.Scripts.Utilities;

namespace Assets.Scripts.Models
{
	public abstract class Tile
		: MonoBehaviour
	{
		public const int LANE_DISTANCE = 2;
		public const int LANES = 3;

		private const int TILES = 5;
		private const float TILE_SIZE = TILES * 6.0f;

		public Transform[] tileVariations;

		public TileType Type { get; protected set; }

		public Orientation Orientation { get; protected set; }

		public virtual Tile Construct(Tile previousTile, TileType type)
		{
			Type = type;
			SetOrientation(previousTile);
			SetPosition(previousTile.transform.position);
			SetRotation();

			if (tileVariations != null && tileVariations.Length > 0)
			{
				AddProps();
			}

			return this;
		}

		private void AddProps()
		{
			Instantiate(tileVariations.Pick(), transform);
		}

		private void SetOrientation(Tile previousTile)
		{
			if (previousTile is LeftCornerTile) {
				Orientation = previousTile.Orientation.GetLeftOrientation();
			}
			else if (previousTile is RightCornerTile) {
				Orientation = previousTile.Orientation.GetRightOrientation();
			}
			else {
				Orientation = previousTile.Orientation;
			}
		}

		private void SetPosition(Vector3 previousPosition)
		{
			transform.position = previousPosition;
			switch (Orientation) {
				case Orientation.North:
					transform.position += Vector3.forward * TILE_SIZE;
					break;
				case Orientation.East:
					transform.position += Vector3.right * TILE_SIZE;
					break;
				case Orientation.South:
					transform.position += Vector3.back * TILE_SIZE;
					break;
				case Orientation.West:
					transform.position += Vector3.left * TILE_SIZE;
					break;
			}
		}

		private void SetRotation()
		{
			transform.rotation = Quaternion.Euler(0, (int)Orientation * 90, 0);
		}
	}
}
