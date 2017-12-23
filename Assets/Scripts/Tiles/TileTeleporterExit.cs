public class TileTeleporterExit : TileBase
{
	public int id;

	protected override void ProtectedAwake()
	{
		base.ProtectedAwake();
		TileTeleporterEntry.CallExitSpawned(this);
	}
}
