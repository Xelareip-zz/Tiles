public class TileBump : TileBase
{	
	public override void TileReached()
	{
		TilePlayer.Instance.ForceTile(TilePlayer.Instance.currentTile);
	}
}
