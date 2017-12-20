using UnityEngine;


public class TileRandom : TileBase
{
	public override void TileReached()
	{
		TileBase target = TileManager.Instance.spawnedTiles[Random.Range(0, TileManager.Instance.spawnedTiles.Count)];
		
		TilePlayer.Instance.Teleport(target);
	}
}
