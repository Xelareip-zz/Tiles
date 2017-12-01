using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLine : MonoBehaviour
{
	public static int lineCount = 0;

	public int lineNumber;

	public int tileCount;
	public int tileOffset;

	public List<TileBase> tiles;

	public TileLine nextLine;
	public TileLine previousLine;

	void Awake()
	{
		lineNumber = ++lineCount;
	}

	public void SpawnTiles()
	{
		for (int i = 0; i < tileCount; ++i)
		{
			TileBase newTile = null;
			if (tiles.Count <= i)
			{
				GameObject tileModel;
				float randValue = Random.Range(0, 90 + 10 * TileManager.Instance.possibleTiles.Count);
				int modelIdx = Mathf.Max(0, Mathf.FloorToInt((randValue - 100.0f) / 10) + 1);
				tileModel = TileManager.Instance.possibleTiles[modelIdx];

				GameObject newTileObj = Instantiate(tileModel, transform.position + Vector3.right * tileOffset * i, transform.rotation, transform);
				newTile = newTileObj.GetComponent<TileBase>();
			}
			else
			{
				newTile = tiles[i];
			}
			newTile.parentLine = this;
			tiles.Add(newTile);
			/*if (nextLine != null)
			{
				nextLine.tiles[i].neighbors[(int)DIRECTIONS.SOUTH] = newTile;
				newTile.neighbors[(int)DIRECTIONS.NORTH] = nextLine.tiles[i];

				if (i != 0)
				{
					newTile.neighbors[(int)DIRECTIONS.NORTH_WEST] = nextLine.tiles[i - 1];
					nextLine.tiles[i - 1].neighbors[(int)DIRECTIONS.SOUTH_EAST] = newTile;
				}
				else if (Parameters.Instance.LoopLeftRight)
				{
					newTile.neighbors[(int)DIRECTIONS.NORTH_WEST] = nextLine.tiles[nextLine.tiles.Count - 1];
					nextLine.tiles[nextLine.tiles.Count - 1].neighbors[(int)DIRECTIONS.SOUTH_EAST] = newTile;
				}
				
				if (i + 1 < nextLine.tiles.Count)
				{
					newTile.neighbors[(int)DIRECTIONS.NORTH_EAST] = nextLine.tiles[i + 1];
					nextLine.tiles[i + 1].neighbors[(int)DIRECTIONS.SOUTH_WEST] = newTile;
				}
			}*/
			if (previousLine != null)
			{
				previousLine.tiles[i].neighbors[(int)DIRECTIONS.NORTH] = newTile;
				newTile.neighbors[(int)DIRECTIONS.SOUTH] = previousLine.tiles[i];

				if (i != 0)
				{
					newTile.neighbors[(int)DIRECTIONS.SOUTH_WEST] = previousLine.tiles[i - 1];
					previousLine.tiles[i - 1].neighbors[(int)DIRECTIONS.NORTH_EAST] = newTile;
				}
				else if (Parameters.Instance.loopLeftRight)
				{
					newTile.neighbors[(int)DIRECTIONS.SOUTH_WEST] = previousLine.tiles[previousLine.tiles.Count - 1];
					previousLine.tiles[previousLine.tiles.Count - 1].neighbors[(int)DIRECTIONS.NORTH_EAST] = newTile;
				}

				if (i + 1 < previousLine.tiles.Count)
				{
					newTile.neighbors[(int)DIRECTIONS.SOUTH_EAST] = previousLine.tiles[i + 1];
					previousLine.tiles[i + 1].neighbors[(int)DIRECTIONS.NORTH_WEST] = newTile;
				}
				else if (Parameters.Instance.loopLeftRight)
				{
					newTile.neighbors[(int)DIRECTIONS.SOUTH_EAST] = previousLine.tiles[0];
					previousLine.tiles[0].neighbors[(int)DIRECTIONS.NORTH_WEST] = newTile;
				}
			}
			if (i != 0)
			{
				tiles[i - 1].neighbors[(int)DIRECTIONS.EAST] = newTile;
				newTile.neighbors[(int)DIRECTIONS.WEST] = tiles[i - 1];
			}
			if (i == tileCount - 1 && Parameters.Instance.loopLeftRight)
			{
				newTile.neighbors[(int)DIRECTIONS.EAST] = tiles[0];
				tiles[0].neighbors[(int)DIRECTIONS.WEST] = newTile;
			}
		}
	}
}
