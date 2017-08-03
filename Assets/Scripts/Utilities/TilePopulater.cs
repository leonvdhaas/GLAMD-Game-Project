using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utilities
{
	public class TilePopulater
	{
		// rows met autowegen voor groundobstacles
		HashSet<int> Row1 = new HashSet<int>() { 0, 3, 6 };
		HashSet<int> Row2 = new HashSet<int>() { 1, 4, 7 };
		HashSet<int> Row3 = new HashSet<int>() { 2, 5, 8 };

		// coin bows over enkele obstakels
		HashSet<int> Bow1 = new HashSet<int>() { 0, 3, 6, 9, 12 };
		HashSet<int> Bow2 = new HashSet<int>() { 1, 4, 7, 10, 13 };
		HashSet<int> Bow3 = new HashSet<int>() { 2, 5, 8, 11, 14 };
		HashSet<int> Bow4 = new HashSet<int>() { 15, 18, 21, 24, 27 };
		HashSet<int> Bow5 = new HashSet<int>() { 16, 19, 22, 25, 28 };
		HashSet<int> Bow6 = new HashSet<int>() { 17, 20, 23, 26, 29 };

		public void Populate(Tile tile)
		{

			if (tile is StraightTile)
			{

			}
		}
	}
}
