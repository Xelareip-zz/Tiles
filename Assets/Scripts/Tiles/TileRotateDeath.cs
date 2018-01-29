using System.Collections;
using UnityEngine;

public class TileRotateDeath : TileBase, IProgress
{
	public float delay;
	public float timeLeft;
	private bool _killMode;
	public GameObject killModeVisual;


	private float GetDelay()
	{
		return delay / TileManager.Instance.GetDifficultyModifier();
	}
	
	protected override void ProtectedUpdate()
	{
		timeLeft -= Time.deltaTime;
		if (timeLeft <= 0)
		{
			timeLeft = GetDelay();
		}
		
		_killMode = timeLeft > GetDelay() / 2.0f;
		killModeVisual.SetActive(_killMode);
		// Means we are on this tile
		if (_killMode && TilePlayer.Instance.LastTile() == this && TilePlayer.Instance.tilesQueue.Count == 0)
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
		
		base.TileReached();
	}

	public float GetProgress()
	{
		return (GetDelay() - timeLeft) / GetDelay();
	}
}
