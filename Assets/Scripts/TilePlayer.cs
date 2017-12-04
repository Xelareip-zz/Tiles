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
	public CameraMove cameraMove;

	public float speed;

	public TileBase currentTile;
	public List<TileBase> tilesQueue = new List<TileBase>();
	public int inputQueueSize;

	public LineRenderer pathLine;

	public List<TileBase> clickableTiles = new List<TileBase>();

	public List<Transform> loopGhosts;

	public int maxDistance;

	public float difficulty;

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
		loopGhosts[0].transform.position = transform.position + Vector3.left * TileManager.Instance.GetWidth();
		loopGhosts[1].transform.position = transform.position + Vector3.right * TileManager.Instance.GetWidth();
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
		difficulty = 0;
	}

	void Start()
	{
		SwipeManager.Instance.Swiped += OnSwipe;
	}

	void OnDestroy()
	{
		SwipeManager.Instance.Swiped -= OnSwipe;
	}

	public float GetSpeed()
	{
		return speed * (difficulty * Parameters.Instance.difficultyIncrease / 100.0f + 1.0f);
	}

	public float GetCameraSpeed()
	{
		return Parameters.Instance.cameraSpeed * (difficulty * Parameters.Instance.difficultyIncrease / 100.0f + 1.0f);
	}

	public TileBase GetRootTile()
	{
		if (tilesQueue.Count > 0)
		{
			return tilesQueue[tilesQueue.Count - 1];
		}
		else
		{
			return currentTile;
		}
	}

	public void OnSwipe(float angle)
	{
		if (Parameters.Instance.swipeControl)
		{
			TileBase rootTile = GetRootTile();
			float bestDot = float.MinValue;
			TileBase bestTarget = null;
			for (int targetIdx = 0; targetIdx < clickableTiles.Count; ++targetIdx)
			{
				TileBase potentialTarget = clickableTiles[targetIdx];
				if (potentialTarget == null)
				{
					continue;
				}

				float neighborAngle = rootTile.NeighborToAngle(potentialTarget);
				float dot = Vector3.Dot(Quaternion.AngleAxis(neighborAngle, Vector3.forward) * Vector3.up, Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up);

				if (dot > bestDot)
				{
					bestDot = dot;
					bestTarget = potentialTarget;
				}
			}

			if (bestTarget != null)
			{
				QueueTile(bestTarget);
			}
		}
	}

	public void MoveLeft()
	{
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
		if (clickableTiles.Contains(rootTile.neighbors[(int)DIRECTIONS.NORTH_WEST]))
		{
			QueueTile(rootTile.neighbors[(int)DIRECTIONS.NORTH_WEST]);
		}
		else if (clickableTiles.Contains(rootTile.neighbors[(int)DIRECTIONS.WEST]))
		{
			QueueTile(rootTile.neighbors[(int)DIRECTIONS.WEST]);
		}
		else if (clickableTiles.Contains(rootTile.neighbors[(int)DIRECTIONS.SOUTH_WEST]))
		{
			QueueTile(rootTile.neighbors[(int)DIRECTIONS.SOUTH_WEST]);
		}
	}

	public void MoveNorth()
	{
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
		if (clickableTiles.Contains(rootTile.neighbors[(int)DIRECTIONS.NORTH]))
		{
			QueueTile(rootTile.neighbors[(int)DIRECTIONS.NORTH]);
		}
		else if (clickableTiles.Contains(rootTile.neighbors[(int)DIRECTIONS.SOUTH]))
		{
			QueueTile(rootTile.neighbors[(int)DIRECTIONS.SOUTH]);
		}
	}

	public void MoveRight()
	{
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
		if (clickableTiles.Contains(rootTile.neighbors[(int)DIRECTIONS.NORTH_EAST]))
		{
			QueueTile(rootTile.neighbors[(int)DIRECTIONS.NORTH_EAST]);
		}
		else if (clickableTiles.Contains(rootTile.neighbors[(int)DIRECTIONS.EAST]))
		{
			QueueTile(rootTile.neighbors[(int)DIRECTIONS.EAST]);
		}
		else if (clickableTiles.Contains(rootTile.neighbors[(int)DIRECTIONS.SOUTH_EAST]))
		{
			QueueTile(rootTile.neighbors[(int)DIRECTIONS.SOUTH_EAST]);
		}
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

	void CheckKeyboard()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			MoveLeft();
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
		{
			MoveNorth();
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			MoveRight();
		}
	}

	void Update ()
	{
		if (CheckDeath())
		{
			EndGame();
			return;
		}
		cameraMove.speed = Vector3.up * GetCameraSpeed();
		difficulty = maxDistance;
		CheckKeyboard();
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

			if (targetMove.magnitude < GetSpeed() * Time.deltaTime)
			{
				transform.position = new Vector3(tilesQueue[0].transform.position.x, tilesQueue[0].transform.position.y, transform.position.z);
				return true;
			}

			transform.position += targetMove.normalized * GetSpeed() * Time.deltaTime;
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
