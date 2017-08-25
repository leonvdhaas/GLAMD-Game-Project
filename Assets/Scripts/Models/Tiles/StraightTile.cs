using Assets.Scripts.Enumerations;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Models.Tiles
{
	public class StraightTile
		: Tile
	{
		public override Tile Construct(Tile previousTile, TileType type)
		{
			base.Construct(previousTile, type);
			TileManager.Instance.TilePopulator.Populate(this);

			return this;
		}
	}
}
