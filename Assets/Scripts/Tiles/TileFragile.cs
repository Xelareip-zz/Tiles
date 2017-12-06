using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileFragile : TileBase
{
	public override void TileLeft()
	{
		Destroy(gameObject);
	}
}
