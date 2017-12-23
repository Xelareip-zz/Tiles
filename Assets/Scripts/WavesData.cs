using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData
{
	[SerializeField]
	public int width;
	[SerializeField]
	public List<string> lines = new List<string>();

	public List<string> GetTiles(int lineIdx)
	{
		return lineIdx >= lines.Count ? null : new List<string>(lines[lineIdx].Split('-'));
	}
}

[CreateAssetMenu]
public class WavesData : ScriptableObject
{
	[SerializeField]
	public List<WaveData> wavesList;

	public void LoadFile(TextAsset textAsset)
	{
		string content = textAsset.text;
		
		wavesList = new List<WaveData>();

		string[] wavesStrings = content.Split('_');
		foreach (string wave in  wavesStrings)
		{
			WaveData newData = new WaveData();

			string cleanWave = wave.Replace('\r', '\n').Replace("\n\n", "\n").Trim('\n');
			
			foreach (string line in cleanWave.Split('\n'))
			{
				newData.lines.Add(line);
			}
			wavesList.Add(newData);
		}
	}
}
