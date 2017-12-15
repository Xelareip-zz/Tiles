using System.Collections.Generic;
using UnityEngine;

public class TileLine : MonoBehaviour
{
	public static int lineCount;
	public static int lineWidth = 1;

	public int lineNumber;

	public List<TileBase> tiles;

	public TileLine previousLine;

	private void Awake()
	{
		lineNumber = ++lineCount;
	}

	public void SpawnTiles(List<TILE_TYPE> forcedTiles)
	{
		lineWidth = forcedTiles.Count;
		
		for (int i = 0; i < forcedTiles.Count; ++i)
		{
			GameObject tileModel = TileManager.Instance.tileToPrefab[forcedTiles[i]];
			Vector3 tilePos = transform.position + Vector3.right * (-Parameters.Parameters.Instance.spaceSize * (lineWidth - 1) / 2.0f + Parameters.Parameters.Instance.spaceSize * i);
			GameObject newTileObj = Instantiate(tileModel, tilePos, transform.rotation, transform);
			TileBase newTile = newTileObj.GetComponent<TileBase>();

			newTile.parentLine = this;
			tiles.Add(newTile);
		}
		SetNeighbors();
	}

	public void SpawnTiles(int width)
	{
		lineWidth = width;
		lineWidth = Mathf.Max(lineWidth, lineWidth);
		
		for (int i = 0; i < lineWidth; ++i)
		{
			TileBase newTile;
			if (tiles.Count <= i)
			{
				int modelIdx = 0;
				if (lineNumber > 5)
				{
					float randValue = Random.Range(0, 90 + 10 * TileManager.Instance.possibleTiles.Count);
					modelIdx = Mathf.Max(0, Mathf.FloorToInt((randValue - 100.0f) / 10) + 1);
				}
				GameObject tileModel = TileManager.Instance.possibleTiles[modelIdx];
				Vector3 tilePos = transform.position + Vector3.right * (-Parameters.Parameters.Instance.spaceSize * (lineWidth - 1) / 2.0f + Parameters.Parameters.Instance.spaceSize * i);
				GameObject newTileObj = Instantiate(tileModel, tilePos, transform.rotation, transform);
				newTile = newTileObj.GetComponent<TileBase>();
			}
			else
			{
				newTile = tiles[i];
			}
			newTile.parentLine = this;
			tiles.Add(newTile);
		}
		SetNeighbors();
	}

	private void SetNeighbors()
	{
		for (int tileIdx = 0; tileIdx < tiles.Count; ++tileIdx)
		{
			TileBase currentTile = tiles[tileIdx];
			if (previousLine != null)
			{
				previousLine.tiles[tileIdx].neighbors[(int)DIRECTIONS.NORTH] = currentTile;
				currentTile.neighbors[(int)DIRECTIONS.SOUTH] = previousLine.tiles[tileIdx];

				if (tileIdx != 0)
				{
					currentTile.neighbors[(int)DIRECTIONS.SOUTH_WEST] = previousLine.tiles[tileIdx - 1];
					previousLine.tiles[tileIdx - 1].neighbors[(int)DIRECTIONS.NORTH_EAST] = currentTile;
				}
				else if (Parameters.Parameters.Instance.loopLeftRight)
				{
					currentTile.neighbors[(int)DIRECTIONS.SOUTH_WEST] = previousLine.tiles[previousLine.tiles.Count - 1];
					previousLine.tiles[previousLine.tiles.Count - 1].neighbors[(int)DIRECTIONS.NORTH_EAST] = currentTile;
				}

				if (tileIdx + 1 < previousLine.tiles.Count)
				{
					currentTile.neighbors[(int)DIRECTIONS.SOUTH_EAST] = previousLine.tiles[tileIdx + 1];
					previousLine.tiles[tileIdx + 1].neighbors[(int)DIRECTIONS.NORTH_WEST] = currentTile;
				}
				else if (Parameters.Parameters.Instance.loopLeftRight)
				{
					currentTile.neighbors[(int)DIRECTIONS.SOUTH_EAST] = previousLine.tiles[0];
					previousLine.tiles[0].neighbors[(int)DIRECTIONS.NORTH_WEST] = currentTile;
				}
			}
			if (tileIdx != 0)
			{
				tiles[tileIdx - 1].neighbors[(int)DIRECTIONS.EAST] = currentTile;
				currentTile.neighbors[(int)DIRECTIONS.WEST] = tiles[tileIdx - 1];
			}
			// ReSharper disable once InvertIf
			if (tileIdx == lineWidth - 1 && Parameters.Parameters.Instance.loopLeftRight)
			{
				currentTile.neighbors[(int)DIRECTIONS.EAST] = tiles[0];
				tiles[0].neighbors[(int)DIRECTIONS.WEST] = currentTile;
			}
		}
	}
}
