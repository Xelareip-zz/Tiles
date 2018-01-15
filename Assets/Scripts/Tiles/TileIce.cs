using System;
using UnityEngine;

public class TileIce : TileBase
{	
	public override void TileReached()
	{

		Quaternion quat = Quaternion.FromToRotation(Vector3.up, transform.position - TilePlayer.Instance.lastPosition);
		DIRECTIONS dir = AngleToDirection(quat.eulerAngles.z);
		
		TilePlayer.Instance.ForceTile(neighbors[(int)dir]);
		
		/*
		Array directions = Enum.GetValues(typeof(DIRECTIONS));
		for (int direction = 0; direction < directions.Length; ++direction)
		{
			if (TilePlayer.Instance.LastTile(1).neighbors[direction] == this)
			{
				TilePlayer.Instance.ForceTile(neighbors[direction]);
				return;
			}
		}*/
	}
}
