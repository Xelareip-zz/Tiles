using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTIONS
{
	NORTH,
	SOUTH,
	EAST,
	WEST,
	NORTH_EAST,
	NORTH_WEST,
	SOUTH_EAST,
	SOUTH_WEST,
}

public enum TILE_TYPE
{
	NORMAL,
	OBSTACLE,
	POINT
}

public class TileBase : MonoBehaviour
{
	public Collider2D coll;

	private TileBase[] _neighbors = new TileBase[8];
	public TileBase[] neighbors
	{
		get
		{
			return _neighbors;
		}
	}

	public GameObject clickableObj;
	public TileLine parentLine;

	void Awake()
	{
	}

	public virtual void TileReached()
	{

	}

	public virtual void TileLeft()
	{

	}

	bool HasInput()
	{
		return Input.GetMouseButtonDown(0) || Input.touchCount != 0;
	}

	Vector3 InputPos()
	{
		if (Input.touchCount != 0)
		{
			return Input.GetTouch(0).position;
		}
		return Input.mousePosition;
	}

	void Update()
	{
		clickableObj.SetActive(TilePlayer.Instance.IsTileCickable(this));

		if (Parameters.Instance.swipeControl == false)
		{
			if (TilePlayer.Instance.IsTileCickable(this))
			{
				if (HasInput())
				{
					Vector3 worldPoint = Camera.main.ScreenToWorldPoint(InputPos());

					if (coll.OverlapPoint(worldPoint))
					{
						TilePlayer.Instance.QueueTile(this);
					}
				}
			}
		}
	}

	public static float DirectionToAngle(DIRECTIONS dir)
	{
		switch(dir)
		{
			case DIRECTIONS.NORTH:
				return 0.0f;
			case DIRECTIONS.NORTH_WEST:
				return 45.0f;
			case DIRECTIONS.WEST:
				return 90.0f;
			case DIRECTIONS.SOUTH_WEST:
				return 135.0f;
			case DIRECTIONS.SOUTH:
				return 180.0f;
			case DIRECTIONS.SOUTH_EAST:
				return 225.0f;
			case DIRECTIONS.EAST:
				return 270.0f;
			case DIRECTIONS.NORTH_EAST:
				return 315.0f;
			default:
				return float.MinValue;
		}
	}

	public float NeighborToAngle(TileBase neighbor)
	{
		if (neighbor == null)
		{
			return float.MinValue;
		}
		for (int neighborIdx = 0; neighborIdx < _neighbors.Length; ++neighborIdx)
		{
			if (neighbors[neighborIdx] == neighbor)
			{
				return DirectionToAngle((DIRECTIONS)neighborIdx);
			}
		}
		return float.MinValue;
	}
}
