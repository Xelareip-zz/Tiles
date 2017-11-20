using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLine : MonoBehaviour
{
	public int tileCount;
	public int tileOffset;

	public List<TileBase> tiles;

	public TileLine nextLine;
	public TileLine previousLine;

	public void SpawnTiles()
	{
		for (int i = 0; i < tileCount; ++i)
		{
			GameObject tileModel;

			float randValue = Random.Range(0, 110);
			if (randValue > 100)
			{
				tileModel = TileManager.Instance.possibleTiles[2];
			}
			else if (randValue > 90)
			{
				tileModel = TileManager.Instance.possibleTiles[1];
			}
			else
			{
				tileModel = TileManager.Instance.possibleTiles[0];
			}

			GameObject newTileObj = Instantiate(tileModel, transform.position + Vector3.right * tileOffset * i, transform.rotation, transform);
			TileBase newTile = newTileObj.GetComponent<TileBase>();
			tiles.Add(newTile);
			if (nextLine != null)
			{
				nextLine.tiles[i].neighbors[(int)DIRECTIONS.SOUTH] = newTile;
				newTile.neighbors[(int)DIRECTIONS.NORTH] = nextLine.tiles[i];
			}
			if (previousLine != null)
			{
				previousLine.tiles[i].neighbors[(int)DIRECTIONS.NORTH] = newTile;
				newTile.neighbors[(int)DIRECTIONS.SOUTH] = previousLine.tiles[i];
			}
			if (i != 0)
			{
				tiles[i - 1].neighbors[(int)DIRECTIONS.EAST] = newTile;
				newTile.neighbors[(int)DIRECTIONS.WEST] = tiles[i - 1];
			}
		}
	}
}
