using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileDeath : TileBase
{
	public GameObject pointsVisual;

	public override void TileReached()
	{
		TilePlayer.Instance.EndGame();
		Destroy(pointsVisual);
	}
}
