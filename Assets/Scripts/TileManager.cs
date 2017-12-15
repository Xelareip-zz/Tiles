using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum WAVES_LIST
{
	BASIC,
	BASIC_5
}

[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
public class TileManager : MonoBehaviour
{
	private static readonly Dictionary<TILE_TYPE, string> TileNames = new Dictionary<TILE_TYPE, string>
	{
		{ TILE_TYPE.NORMAL, "TileDefault" },
		{ TILE_TYPE.OBSTACLE, "TileDeath" },
		{ TILE_TYPE.POINT, "TilePoint" }
	};
	
	private static readonly Dictionary<WAVES_LIST, string> WavesNames = new Dictionary<WAVES_LIST, string>
	{
		{ WAVES_LIST.BASIC, "WavesBasic" },
		{ WAVES_LIST.BASIC_5, "WavesBasic5" }
	};
	
	public static TileManager Instance { get; private set; }

	public List<GameObject> possibleLines;
	public List<GameObject> possibleTiles;

	public List<TileLine> tileLines = new List<TileLine>();

	//private float _killLimit;
	public float lineOffset;

	public List<WavesData> dataList;
	public WavesData data;

	public WaveData currentWave;
	public int currentLine;

	//public GameObject killLine;

	public Dictionary<TILE_TYPE, GameObject> tileToPrefab;

	private void Awake()
	{
		Instance = this;
		TileLine.lineCount = 0;

		tileToPrefab = new Dictionary<TILE_TYPE, GameObject>();
		foreach (TILE_TYPE tileType in System.Enum.GetValues(typeof(TILE_TYPE)))
		{
			if (!TileNames.ContainsKey(tileType))
			{
				continue;
			}
			tileToPrefab.Add(tileType, possibleTiles[0]);

			for (int prefabIdx = 0; prefabIdx < possibleTiles.Count; ++prefabIdx)
			{
				if (possibleTiles[prefabIdx].name == TileNames[tileType])
				{
					tileToPrefab[tileType] = possibleTiles[prefabIdx];
				}
			}
		}

		for (int dataIdx = 0; dataIdx < dataList.Count; ++dataIdx)
		{
			if (dataList[dataIdx].name != WavesNames[Parameters.Parameters.Instance.wavesStyle])
			{
				continue;
			}
			data = dataList[dataIdx];
			break;
		}
		
		FindWave();
	}

	private void Start()
	{
		lineOffset = Parameters.Parameters.Instance.spaceSize;
		//_killLimit = (Parameters.Parameters.Instance.deathHeight / 100.0f - 0.5f) * Camera.main.orthographicSize * 2.0f;
		//if (Parameters.Parameters.Instance.mustTiles == false)
		//{
		//	for (int i = 0; i < possibleTiles.Count; ++i)
		//	{
		//		if (possibleTiles[i].gameObject.name != "TileMust")
		//		{
		//			continue;
		//		}
		//		possibleTiles.RemoveAt(i);
		//		break;
		//	}
//
		//}
		//if (Parameters.Parameters.Instance.fragileTiles == false)
		//{
		//	for (int i = 0; i < possibleTiles.Count; ++i)
		//	{
		//		if (possibleTiles[i].gameObject.name != "TileFragile")
		//		{
		//			continue;
		//		}
		//		possibleTiles.RemoveAt(i);
		//		break;
		//	}
		//}
		for (int lineIdx = 0; lineIdx < tileLines.Count; ++lineIdx)
		{
			tileLines[lineIdx].SpawnTiles(currentWave.width);
		}
		SpawnLines();
		//if (Parameters.Parameters.Instance.autoMove)
		//{
		//	Destroy(killLine);
		//}
		//else
		//{
		//	killLine.transform.position = new Vector3(killLine.transform.position.x, transform.position.y + _killLimit, killLine.transform.position.z);
		//	killLine.transform.localScale = new Vector3(TileLine.lineWidth * Parameters.Parameters.Instance.spaceSize * 2.0f, killLine.transform.localScale.y, killLine.transform.localScale.z);
		//}
		
		TilePlayer.Instance.FindTile();
		TilePlayer.Instance.transform.position = new Vector3(TilePlayer.Instance.currentTile.transform.position.x, TilePlayer.Instance.currentTile.transform.position.y, TilePlayer.Instance.transform.position.z);
    }

	private void FindWave()
	{
		currentLine = 0;
		currentWave = data.wavesList[Random.Range(0, data.wavesList.Count)];
	}

	public static float GetWidth()
	{
		return TileLine.lineWidth * Parameters.Parameters.Instance.spaceSize;
	}

	private void Update()
	{
		SpawnLines();
		ClearTiles();
	}

	//public float KillHeight()
	//{
	//	return transform.position.y + _killLimit;
	//}

	private void ClearTiles()
	{
		for (int lineIdx = 0; lineIdx < tileLines.Count - 1; ++lineIdx)
		{
			if (tileLines[lineIdx].transform.position.y < transform.position.y - TileCamera.Instance.camera.orthographicSize * 2.0f)
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

	private void SpawnLines()
	{
		while (tileLines[tileLines.Count - 1].transform.position.y < transform.position.y + TileCamera.Instance.camera.orthographicSize)
		{
			GameObject newLineObj = Instantiate(possibleLines[0], tileLines[tileLines.Count - 1].transform.position + Vector3.up * lineOffset, Quaternion.identity);
			TileLine newLine = newLineObj.GetComponent<TileLine>();
			newLine.previousLine = tileLines[tileLines.Count - 1];
			if (TileLine.lineCount > 5)
			{
				List<TILE_TYPE> tiles = currentWave.GetTiles(currentLine);
				if (tiles == null)
				{
					FindWave();
					tiles = currentWave.GetTiles(currentLine);
				}
				newLine.SpawnTiles(tiles);
				++currentLine;
			}
			else
			{
				newLine.SpawnTiles(currentWave.width);
			}
            tileLines.Add(newLine);
		}
	}
}
