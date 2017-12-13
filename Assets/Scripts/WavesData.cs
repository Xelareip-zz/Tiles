using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData
{
	[SerializeField]
	public int width;
	[SerializeField]
	public List<string> lines = new List<string>();

	public List<TILE_TYPE> GetTiles(int lineIdx)
	{
		if (lineIdx >= lines.Count)
		{
			return null;
		}

		List<TILE_TYPE> res = new List<TILE_TYPE>();

		string[] tileStringsTab = lines[lineIdx].Split('-');

		for (int tileId = 0; tileId < width; ++tileId)
		{
			TILE_TYPE type = TILE_TYPE.NORMAL;
			if (tileId < tileStringsTab.Length && string.IsNullOrEmpty(tileStringsTab[tileId]) == false)
			{
				type = (TILE_TYPE)System.Enum.Parse(typeof(TILE_TYPE), tileStringsTab[tileId]);
			}

			res.Add(type);
		}

		return res;
	}
}

[CreateAssetMenu]
public class WavesData : ScriptableObject
{
	[SerializeField]
	public List<WaveData> wavesList;

}
