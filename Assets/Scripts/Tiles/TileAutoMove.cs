using UnityEngine;


public class TileAutoMove : TileBase
{
	public DIRECTIONS direction;
	public GameObject visual;

	private void Start()
	{
		visual.transform.rotation = Quaternion.AngleAxis(45.0f * (int)direction, Vector3.forward);
	}
	
	public override void TileReached()
	{
		TilePlayer.Instance.ForceTile(neighbors[(int) direction]);
		
		base.TileReached();
	}
}
