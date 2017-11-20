using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlayer : MonoBehaviour
{
	private static TilePlayer instance;
	public static TilePlayer Instance
	{
		get
		{
			return instance;
		}
	}

	public GameObject endGame;

	public float speed;

	public TileBase targetTile;
	public TileBase nextTile;

	public List<TileBase> clickableTiles = new List<TileBase>();

	public bool TileReached()
	{
		if (targetTile == null)
		{
			return true;
		}
		return transform.position.x == targetTile.transform.position.x && transform.position.y == targetTile.transform.position.y;
    }

	void Awake()
	{
		instance = this;
	}

	public void FindTile()
	{
		float minDist = float.MaxValue;
		TileBase closestTile = null;
		for (int lineIdx = 0; lineIdx < TileManager.Instance.tileLines.Count; ++lineIdx)
		{
			TileLine currentLine = TileManager.Instance.tileLines[lineIdx];
			for (int tileIdx = 0; tileIdx < currentLine.tiles.Count; ++tileIdx)
			{
				TileBase currentTile = currentLine.tiles[tileIdx];

				float dist = Vector3.Distance(currentTile.transform.position, transform.position);
				if (dist < minDist)
				{
					minDist = dist;
					closestTile = currentTile;
				}
			}
		}

		targetTile = closestTile;
	}

	void Update ()
	{
		if (CheckDeath())
		{
			EndGame();
			return;
		}

		if (GoToTile())
		{
			targetTile.TileReached();
		}

		ActivateClickableTiles();
	}

	public void EndGame()
	{
		endGame.SetActive(true);
		Destroy(gameObject);
	}

	bool CheckDeath()
	{
		return transform.position.y < TileManager.Instance.KillHeight();
	}
	
	void ActivateClickableTiles()
	{
		clickableTiles.Clear();
		if (TileReached() == false)
		{
			return;
		}

		if (targetTile != null)
		{
			for (int i = 0; i < 4; ++i)
			{
				TileBase neighbor = targetTile.neighbors[i];
				clickableTiles.Add(neighbor);
			}
		}
	}

	public bool IsTileCickable(TileBase tile)
	{
		return clickableTiles.Contains(tile);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.tag == "Wall")
		{
			endGame.SetActive(true);
			Destroy(gameObject);
		}
	}

	bool GoToTile()
	{
		if (TileReached() == false)
		{
			Vector3 targetMove = targetTile.transform.position - transform.position;

			targetMove.z = 0;

			if (targetMove.magnitude < speed * Time.deltaTime)
			{
				transform.position = targetTile.transform.position;
				return true;
			}

			transform.position += targetMove.normalized * speed * Time.deltaTime;
		}
		return false;
	}
}
