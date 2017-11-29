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

	public TileBase currentTile;
	public List<TileBase> tilesQueue = new List<TileBase>();
	public int inputQueueSize;

	public LineRenderer pathLine;

	public List<TileBase> clickableTiles = new List<TileBase>();

	public List<Transform> loopGhosts;

	public int maxDistance;

	public bool TileReached()
	{
		if (tilesQueue.Count > 0)
		{
			return transform.position.x == tilesQueue[0].transform.position.x && transform.position.y == tilesQueue[0].transform.position.y;
		}
		return true;
    }

	void Awake()
	{
		instance = this;
		speed = Parameters.Instance.playerSpeed;
		if (Parameters.Instance.inputQueueSize < 0)
		{
			inputQueueSize = int.MaxValue;
		}
		else
		{
			inputQueueSize = Parameters.Instance.inputQueueSize;
		}
		maxDistance = 0;
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

		currentTile = closestTile;
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
			int lineNumber = tilesQueue[0].parentLine.lineNumber;
            if (Parameters.Instance.pointsPerLine && lineNumber > maxDistance)
			{
				ScoreManager.Instance.score += lineNumber - maxDistance;
				maxDistance = lineNumber;
            }
			tilesQueue[0].TileReached();
			currentTile = tilesQueue[0];
			if (tilesQueue.Count > 0)
			{
				tilesQueue.RemoveAt(0);
			}
			FindBestGhost();
		}

		ActivateClickableTiles();
		DrawPath();
	}

	public void FindBestGhost()
	{
		if (tilesQueue.Count > 0)
		{
			float bestDist = Vector3.Distance(transform.position, tilesQueue[0].transform.position);
			Vector3 bestPos = transform.position;
			for (int ghostId = 0; ghostId < loopGhosts.Count; ++ghostId)
			{
				float dist = Vector3.Distance(loopGhosts[ghostId].transform.position, tilesQueue[0].transform.position);

				if (dist < bestDist)
				{
					bestDist = dist;
					bestPos = loopGhosts[ghostId].transform.position;
				}
			}

			transform.position = bestPos;
		}
	}

	public void DrawPath()
	{
		if (Parameters.Instance.drawPath == false)
		{
			return;
		}
		if (tilesQueue.Count > 0)
		{
			pathLine.enabled = true;
			Vector3[] pathPoints = new Vector3[tilesQueue.Count + 1];
			pathPoints[0] = transform.position;

			for (int i = 0; i < tilesQueue.Count; ++i)
			{
				pathPoints[i + 1] = tilesQueue[i].transform.position;
			}
			pathLine.positionCount = tilesQueue.Count + 1;
			pathLine.SetPositions(pathPoints);
		}
		else
		{
			pathLine.enabled = false;
		}
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

	bool AllowedDirection(DIRECTIONS dir)
	{
		switch(dir)
		{
			case DIRECTIONS.NORTH:
				return Parameters.Instance.moveNorth;
			case DIRECTIONS.SOUTH:
				return Parameters.Instance.moveSouth;
			case DIRECTIONS.EAST:
				return Parameters.Instance.moveEast;
			case DIRECTIONS.WEST:
				return Parameters.Instance.moveWest;
			case DIRECTIONS.NORTH_EAST:
				return Parameters.Instance.moveNorthEast;
			case DIRECTIONS.NORTH_WEST:
				return Parameters.Instance.moveNorthWest;
			case DIRECTIONS.SOUTH_EAST:
				return Parameters.Instance.moveSouthEast;
			case DIRECTIONS.SOUTH_WEST:
				return Parameters.Instance.moveSouthWest;
		}
		return false;
	}

	public void QueueTile(TileBase tile)
	{
		if (tilesQueue.Count < inputQueueSize)
		{
			tilesQueue.Add(tile);
			if (tilesQueue.Count == 1)
			{
				FindBestGhost();
			}
		}
	}
	
	void ActivateClickableTiles()
	{
		clickableTiles.Clear();

		TileBase rootTile;
		if (tilesQueue.Count > inputQueueSize - 1)
		{
			return;
		}
		else if (tilesQueue.Count > 0)
		{
			rootTile = tilesQueue[tilesQueue.Count - 1];
		}
		else
		{
			rootTile = currentTile;
		}

		if (currentTile != null)
		{
			for (int i = 0; i < rootTile.neighbors.Length; ++i)
			{
				if (AllowedDirection((DIRECTIONS) i))
				{
					TileBase neighbor = rootTile.neighbors[i];
					clickableTiles.Add(neighbor);
				}
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
			Vector3 targetMove = tilesQueue[0].transform.position - transform.position;

			targetMove.z = 0;

			if (targetMove.magnitude < speed * Time.deltaTime)
			{
				transform.position = new Vector3(tilesQueue[0].transform.position.x, tilesQueue[0].transform.position.y, transform.position.z);
				return true;
			}

			transform.position += targetMove.normalized * speed * Time.deltaTime;
		}
		return false;
	}

	void OnDrawGizmos()
	{
		if (tilesQueue.Count > 0)
		{
			Gizmos.DrawLine(transform.position, tilesQueue[0].transform.position);

			for (int i = 0; i < tilesQueue.Count - 1; ++i)
			{
				Gizmos.DrawLine(tilesQueue[i].transform.position, tilesQueue[i + 1].transform.position);
			}
        }
	}
}
