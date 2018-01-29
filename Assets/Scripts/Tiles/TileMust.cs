using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileMust : TileBase
{
	public bool reached = false;
	public GameObject reachedVisual;

	public override void TileReached()
	{
		reached = true;
		Destroy(reachedVisual);
		
		base.TileReached();
	}

	void OnDestroy()
	{
		if (TilePlayer.Instance != null && reached == false)
		{
			TilePlayer.Instance.EndGame();
		}
	}
}
