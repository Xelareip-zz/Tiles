using System.Collections;
using UnityEngine;

public class TileTimedDeath : TileBase, IProgress
{
	private bool _triggered;
	public float delay;
	private float _timeLeft;
	private bool _killMode;
	public GameObject killModeVisual;
	
	private float GetDelay()
	{
		return delay / TileManager.Instance.GetDifficultyModifier();
	}
	
	public override void TileReached()
	{
		if (_killMode)
		{
			TilePlayer.Instance.EndGame();	
		}
		else
		{
			_triggered = true;
		}
	}

	protected override void ProtectedAwake()
	{
		_timeLeft = GetDelay();
	}

	protected override void ProtectedUpdate()
	{
		if (_triggered == false)
		{
			return;
		}
		
		_timeLeft -= Time.deltaTime;
		if (_timeLeft <= 0)
		{
			_killMode = true;
			killModeVisual.SetActive(true);
			if (TilePlayer.Instance.LastTile() == this && TilePlayer.Instance.tilesQueue.Count == 0)
			{
				TilePlayer.Instance.EndGame();
			}
		}
	}

	public float GetProgress()
	{
		return (delay -_timeLeft) / delay;
	}
}
