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
}

[CreateAssetMenu]
public class WavesData : ScriptableObject
{
	[SerializeField]
	public List<WaveData> wavesList;
}
