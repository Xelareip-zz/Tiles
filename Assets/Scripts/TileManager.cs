using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;


[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
public class TileManager : MonoBehaviour
{	
	public static TileManager Instance { get; private set; }

	public List<GameObject> possibleLines;
	public List<GameObject> possibleTiles;

	public List<TileLine> tileLines = new List<TileLine>();

	public float lineOffset;

	public WavesData data;

	public WaveData currentWave;
	public int currentLine;

	public Dictionary<string, GameObject> tileToPrefab;

	public List<TileBase> spawnedTiles;

	private void Awake()
	{
		Instance = this;
		spawnedTiles = new List<TileBase>();
		TileLine.lineCount = 0;

		possibleTiles = Resources.LoadAll<GameObject>("Tiles").ToList();
		tileToPrefab = new Dictionary<string, GameObject>();
		for (int tileIdx = 0; tileIdx < possibleTiles.Count; ++tileIdx)
		{
			tileToPrefab.Add(possibleTiles[tileIdx].name, possibleTiles[tileIdx]);
		}

		data = Resources.Load<WavesData>("Levels/" + Parameters.Parameters.Instance.levels);
		
		FindWave();
	}

	private void Start()
	{
		lineOffset = Parameters.Parameters.Instance.spaceSize;
		for (int lineIdx = 0; lineIdx < tileLines.Count; ++lineIdx)
		{
			tileLines[lineIdx].SpawnTiles(currentWave.width);
		}
		SpawnLines();
		
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
		while (tileLines[tileLines.Count - 1].transform.position.y < Mathf.Max(TilePlayer.Instance.transform.position.y, transform.position.y) + TileCamera.Instance.camera.orthographicSize)
		{
			GameObject newLineObj = Instantiate(possibleLines[0], tileLines[tileLines.Count - 1].transform.position + Vector3.up * lineOffset, Quaternion.identity);
			TileLine newLine = newLineObj.GetComponent<TileLine>();
			newLine.previousLine = tileLines[tileLines.Count - 1];
			if (TileLine.lineCount > 5)
			{
				List<string> tiles = currentWave.GetTiles(currentLine);
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
