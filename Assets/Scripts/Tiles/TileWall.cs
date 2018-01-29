using UnityEngine;

public class TileWall : MonoBehaviour
{
	public Collider2D collider;
	public ContactFilter2D contactFilter;

	private void Update()
	{
		if (collider.IsTouching(TilePlayer.Instance.collider))
		{
			TilePlayer.Instance.ForceTile(TilePlayer.Instance.LastTile(), false);			
		}
	}
}
