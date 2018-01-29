using System;

public class TileTeleporterEntry : TileBase
{
	public static event Action<TileTeleporterExit> exitSpawned;

	public static void CallExitSpawned(TileTeleporterExit exit)
	{
		if (exitSpawned != null)
		{
			exitSpawned(exit);	
		}
	}
	
	public int id;
	private TileTeleporterExit target;

	protected override void ProtectedAwake()
	{
		base.ProtectedAwake();
		exitSpawned += ExitSpawned;
	}

	protected override void ProtectedOnDestroy()
	{
		base.ProtectedOnDestroy();
		exitSpawned -= ExitSpawned;
	}

	private void ExitSpawned(TileTeleporterExit exit)
	{
		// ReSharper disable once InvertIf
		if (target == null && exit.id == id)
		{
			target = exit;
			exitSpawned -= ExitSpawned;
		}
	}

	public override void TileReached()
	{
		TilePlayer.Instance.Teleport(target);
		
		base.TileReached();
	}
}
