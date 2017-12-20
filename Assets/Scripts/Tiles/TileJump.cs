public class TileJump : TileBase
{
	public override void TileReached()
	{
		TileBase target = neighbors[(int)DIRECTIONS.NORTH].neighbors[(int)DIRECTIONS.NORTH];
		TilePlayer.Instance.Teleport(target);
	}
}
