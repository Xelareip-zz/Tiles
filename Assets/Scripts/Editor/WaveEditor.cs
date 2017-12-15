using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	public class TileDescription
	{
		public Texture2D image;

		public static TileDescription Build(string prefabPath)
		{
			TileDescription newDesc = new TileDescription();
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
		

			newDesc.image = AssetPreview.GetAssetPreview(prefab);
			return newDesc;
		}
	}

	[CustomEditor(typeof(WavesData))]
	public class WaveEditor : UnityEditor.Editor
	{
		private static Dictionary<TILE_TYPE, TileDescription> _tileDescriptions;

		private WavesData _data;

		private int _foldoutId;

		private void OnEnable()
		{
			if (_tileDescriptions == null)
			{
				SetDescriptions();
			}
			_data = (WavesData)target;
			if (_data.wavesList == null)
			{
				_data.wavesList = new List<WaveData>();
			}
		}

		private static void SetDescriptions()
		{
			_tileDescriptions = new Dictionary<TILE_TYPE, TileDescription>()
			{
				{ TILE_TYPE.NORMAL, TileDescription.Build("Assets/Prefabs/TileDefault.prefab") },
				{ TILE_TYPE.OBSTACLE, TileDescription.Build("Assets/Prefabs/TileDeath.prefab") },
				{ TILE_TYPE.POINT, TileDescription.Build("Assets/Prefabs/TilePoint.prefab") }
			};

		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.BeginVertical();
			if (GUILayout.Button("Refresh"))
			{
				SetDescriptions();
			}

			EditorGUILayout.BeginHorizontal();
			foreach (KeyValuePair<TILE_TYPE, TileDescription> kvp in _tileDescriptions)
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
				_data.wavesList.Add(new WaveData());
			}
			if (GUILayout.Button("Save"))
			{
				EditorUtility.SetDirty(_data);
				AssetDatabase.SaveAssets();
			}
			EditorGUILayout.EndHorizontal();
			for (int waveIdx = 0; waveIdx < _data.wavesList.Count; ++waveIdx)
			{
				EditorGUILayout.BeginHorizontal();
				bool foldout = EditorGUILayout.Foldout(waveIdx == _foldoutId, "Wave " + waveIdx);
				if (GUILayout.Button("X"))
				{
					_data.wavesList.RemoveAt(waveIdx);
					return;
				}
				EditorGUILayout.EndHorizontal();
				if (foldout)
				{
					_foldoutId = waveIdx;
					WaveData wave = _data.wavesList[waveIdx];
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
								type = (TILE_TYPE)Enum.Parse(typeof(TILE_TYPE), tileStringsTab[tileId]);
							}

							tileStrings.Add(type.ToString());

							if (GUILayout.Button(_tileDescriptions[type].image))
							{
								type = (TILE_TYPE)(((int)type + 1) % Enum.GetValues(typeof(TILE_TYPE)).Length);
								tileStrings[tileId] = type.ToString();
							}
						}

						string newString = string.Join("-", tileStrings.ToArray());
						// ReSharper disable once RedundantCheckBeforeAssignment
						if (_data.wavesList[waveIdx].lines[lineIdx] != newString)
						{
							_data.wavesList[waveIdx].lines[lineIdx] = newString;
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
}