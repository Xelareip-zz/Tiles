using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TilePoint : TileBase
{
	public int points;
	public GameObject pointsVisual;

	public override void TileReached()
	{
		ScoreManager.Instance.score += points;
		points = 0;
		Destroy(pointsVisual);
		
		base.TileReached();
	}
}
