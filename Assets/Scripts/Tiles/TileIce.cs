using System;

public class TileIce : TileBase
{	
	public override void TileReached()
	{
		Array directions = Enum.GetValues(typeof(DIRECTIONS));
		for (int direction = 0; direction < directions.Length; ++direction)
		{
			if (TilePlayer.Instance.LastTile(1).neighbors[direction] == this)
			{
				TilePlayer.Instance.ForceTile(neighbors[direction]);
				return;
			}
		}
	}
}
