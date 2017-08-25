using Assets.Scripts.Enumerations;

namespace Assets.Scripts.Models.Tiles
{
	public class StartTile
		: Tile
	{
		private void Start()
		{
			Type = TileType.Regular;
			Orientation = Orientation.North;
		}
	}
}
