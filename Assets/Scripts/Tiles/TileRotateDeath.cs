using System.Collections;
using UnityEngine;

public class TileRotateDeath : TileBase
{
	public float delay;
	public float offset;
	private bool _killMode;
	public GameObject killModeVisual;

	protected override void ProtectedAwake()
	{
		base.ProtectedAwake();
		
		StartCoroutine(RotateKilling());
	}

	private IEnumerator RotateKilling()
	{
		yield return new WaitForSeconds(offset);
		while (true)
		{
			yield return new WaitForSeconds(delay / 2.0f);
			_killMode = !_killMode;
			killModeVisual.SetActive(_killMode);
			// Means we are on this tile
			if (_killMode && TilePlayer.Instance.LastTile() == this && TilePlayer.Instance.tilesQueue.Count == 0)
			{
				TilePlayer.Instance.EndGame();
			}	
		}
	}
	
	public override void TileReached()
	{
		if (_killMode)
		{
			TilePlayer.Instance.EndGame();	
		}
	}
}
