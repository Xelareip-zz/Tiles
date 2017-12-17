using UnityEngine;


public class TileRandom : TileBase
{
	//public int points;
	//public GameObject visual;
	//public GameObject target;

	public override void TileReached()
	{
		TileBase target = TileManager.Instance.spawnedTiles[Random.Range(0, TileManager.Instance.spawnedTiles.Count)];
		
		TilePlayer.Instance.transform.position = new Vector3(target.transform.position.x - 0.01f, target.transform.position.y, TilePlayer.Instance.transform.position.z);
		TilePlayer.Instance.ForceTile(target);
	}
}
