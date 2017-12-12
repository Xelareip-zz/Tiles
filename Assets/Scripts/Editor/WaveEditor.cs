using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TileDescription
{
	public Texture2D image;
	public GameObject prefab;

	public static TileDescription Build(string texturePath, string prefabPath)
	{
		TileDescription newDesc = new TileDescription();
		Texture2D image = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
		GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
		

		newDesc.image = AssetPreview.GetAssetPreview(prefab);
		newDesc.prefab = prefab;
		return newDesc;
	}
}

[CustomEditor(typeof(WavesData))]
public class WaveEditor : Editor
{
	private static Dictionary<TILE_TYPE, TileDescription> tileDescriptions = null;

	private Dictionary<string, Vector2> scrolls = new Dictionary<string, Vector2>();

	private WavesData data;

	private int foldoutId = 0;

	void OnEnable()
	{
		if (tileDescriptions == null)
		{
			SetDescriptions();
		}
		data = (WavesData)target;
		if (data.wavesList == null)
		{
			data.wavesList = new List<WaveData>();
		}
	}

	private void SetDescriptions()
	{
		tileDescriptions = new Dictionary<TILE_TYPE, TileDescription>()
			{
				{ TILE_TYPE.NORMAL, TileDescription.Build("Assets/Sprites/White.png", "Assets/Prefabs/TileDefault.prefab") },
				{ TILE_TYPE.OBSTACLE, TileDescription.Build("Assets/Sprites/Cross.png", "Assets/Prefabs/TileDeath.prefab") },
				{ TILE_TYPE.POINT, TileDescription.Build("Assets/Sprites/WhiteRound.png", "Assets/Prefabs/TilePoint.prefab") }
			};
	}

	private Vector2 GetScroll(string key)
	{
		if (scrolls.ContainsKey(key) == false)
		{
			scrolls.Add(key, Vector2.zero);
		}
		return scrolls[key];
	}

	private void SetScroll(string key, Vector2 val)
	{
		if (scrolls.ContainsKey(key) == false)
		{
			scrolls.Add(key, val);
		}
		scrolls[key] = val;
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginVertical();
		if (GUILayout.Button("Refresh"))
		{
			SetDescriptions();
		}

		EditorGUILayout.BeginHorizontal();
		foreach (var kvp in tileDescriptions)
		{
			EditorGUILayout.BeginVertical();
			GUILayout.Label(kvp.Key.ToString());
			GUILayout.Label(kvp.Value.image);
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Add"))
		{
			data.wavesList.Add(new WaveData());
		}
		if (GUILayout.Button("Save"))
		{
			EditorUtility.SetDirty(data);
			AssetDatabase.SaveAssets();
		}
		EditorGUILayout.EndHorizontal();
		for (int waveIdx = 0; waveIdx < data.wavesList.Count; ++waveIdx)
		{
			bool foldout = EditorGUILayout.Foldout(waveIdx == foldoutId, "Wave " + waveIdx);
			if (foldout)
			{
				foldoutId = waveIdx;
				WaveData wave = data.wavesList[waveIdx];
				wave.width = Mathf.Clamp(EditorGUILayout.IntField(wave.width), 2, 10);

                for (int lineIdx = 0; lineIdx < wave.lines.Count; ++lineIdx)
				{
					string[] tileStringsTab = wave.lines[lineIdx].Split('-');

					List<string> tileStrings = new List<string>();

                    EditorGUILayout.BeginHorizontal();

					for (int tileId = 0; tileId < wave.width; ++tileId)
					{
						TILE_TYPE type = TILE_TYPE.NORMAL;
                        if (tileId < tileStringsTab.Length && string.IsNullOrEmpty(tileStringsTab[tileId]) == false)
						{
							type = (TILE_TYPE)System.Enum.Parse(typeof(TILE_TYPE), tileStringsTab[tileId]);
						}

						tileStrings.Add(type.ToString());

						if (GUILayout.Button(tileDescriptions[type].image))
						{
							type = (TILE_TYPE)(((int)type + 1) % Enum.GetValues(typeof(TILE_TYPE)).Length);
							tileStrings[tileId] = type.ToString();
						}
					}

					string newString = string.Join("-", tileStrings.ToArray());
					if (data.wavesList[waveIdx].lines[lineIdx] != newString)
					{
						data.wavesList[waveIdx].lines[lineIdx] = newString;
					}

					EditorGUILayout.EndHorizontal();
				}

				if (GUILayout.Button("Add Line"))
                {
					wave.lines.Add("");
				}
			}
		}
		EditorGUILayout.EndVertical();
	}
}
