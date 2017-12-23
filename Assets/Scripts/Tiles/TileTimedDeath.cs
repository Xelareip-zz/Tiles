using System.Collections;
using UnityEngine;

public class TileTimedDeath : TileBase
{
	public float delay;
	private bool _killMode;
	public GameObject killModeVisual;
	
	private IEnumerator KillInDelay()
	{
		yield return new WaitForSeconds(delay);
		killModeVisual.SetActive(true);
		_killMode = true;
		// Means we are on this tile
		if (TilePlayer.Instance.LastTile() == this && TilePlayer.Instance.tilesQueue.Count == 0)
		{
			TilePlayer.Instance.EndGame();
		}
	}
	
	public override void TileReached()
	{
		if (_killMode)
		{
			TilePlayer.Instance.EndGame();	
		}
		else
		{
			StartCoroutine(KillInDelay());
		}
	}
}
