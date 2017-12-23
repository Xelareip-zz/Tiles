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
}
