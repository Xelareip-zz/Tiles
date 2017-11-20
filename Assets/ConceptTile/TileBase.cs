using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTIONS
{
	NORTH,
	SOUTH,
	EAST,
	WEST
};

public class TileBase : MonoBehaviour
{
	public Collider2D coll;

	public TileBase[] neighbors = new TileBase[4];

	public GameObject clickableObj;

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
					TilePlayer.Instance.targetTile = this;
				}
			}
		}
	}
}
