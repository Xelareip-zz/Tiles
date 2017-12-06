using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	private static TileManager instance;
	public static TileManager Instance
	{
		get
		{
			return instance;
		}
	}

	public List<GameObject> possibleLines;
	public List<GameObject> possibleTiles;

	public List<TileLine> tileLines = new List<TileLine>();

	private float killLimit;
	public float spawnLimit;
	public float lineOffset;

	public GameObject killLine;

	void Awake()
	{
		instance = this;
		TileLine.lineCount = 0;
	}

	void Start()
	{
		lineOffset = Parameters.Instance.spaceSize;
		killLimit = (Parameters.Instance.deathHeight / 100.0f - 0.5f) * Camera.main.orthographicSize * 2.0f;
		if (Parameters.Instance.mustTiles == false)
		{
			for (int i = 0; i < possibleTiles.Count; ++i)
			{
				if (possibleTiles[i].gameObject.name == "TileMust")
				{
					possibleTiles.RemoveAt(i);
					break;
				}
			}

		}
		if (Parameters.Instance.fragileTiles == false)
		{
			for (int i = 0; i < possibleTiles.Count; ++i)
			{
				if (possibleTiles[i].gameObject.name == "TileFragile")
				{
					possibleTiles.RemoveAt(i);
					break;
				}
			}

		}
		killLine.transform.position = new Vector3(killLine.transform.position.x, transform.position.y + killLimit, killLine.transform.position.z);
		killLine.transform.localScale = new Vector3(Parameters.Instance.width * Parameters.Instance.spaceSize * 2.0f, killLine.transform.localScale.y, killLine.transform.localScale.z);
		for (int lineIdx = 0; lineIdx < tileLines.Count; ++lineIdx)
		{
			tileLines[lineIdx].SpawnTiles();
		}
		SpawnLines();

		TilePlayer.Instance.FindTile();
		TilePlayer.Instance.transform.position = new Vector3(TilePlayer.Instance.currentTile.transform.position.x, TilePlayer.Instance.currentTile.transform.position.y, TilePlayer.Instance.transform.position.z);
    }

	public float GetWidth()
	{
		return Parameters.Instance.width * Parameters.Instance.spaceSize;
	}

	void Update()
	{
		SpawnLines();
		ClearTiles();
	}

	public float KillHeight()
	{
		return transform.position.y + killLimit;
	}

	void ClearTiles()
	{
		for (int lineIdx = 0; lineIdx < tileLines.Count - 1; ++lineIdx)
		{
			if (tileLines[lineIdx].transform.position.y < transform.position.y + killLimit)
			{
				Destroy(tileLines[lineIdx].gameObject);
				tileLines.RemoveAt(lineIdx);
				--lineIdx;
			}
			else
			{
				break;
			}
		}
	}

	void SpawnLines()
	{
		while (tileLines[tileLines.Count - 1].transform.position.y < transform.position.y + TileCamera.Instance.camera.orthographicSize)
		{
			GameObject newLineObj = Instantiate(possibleLines[0], tileLines[tileLines.Count - 1].transform.position + Vector3.up * lineOffset, Quaternion.identity);
			TileLine newLine = newLineObj.GetComponent<TileLine>();
			tileLines[tileLines.Count - 1].nextLine = newLine;
			newLine.previousLine = tileLines[tileLines.Count - 1];
			newLine.SpawnTiles();
            tileLines.Add(newLine);
		}
	}
}
