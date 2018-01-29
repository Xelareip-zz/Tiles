public class TileDeath : TileBase
{
	public override void TileReached()
	{
		TilePlayer.Instance.EndGame();
		
		base.TileReached();
	}
}
