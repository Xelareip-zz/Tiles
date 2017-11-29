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
};

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
