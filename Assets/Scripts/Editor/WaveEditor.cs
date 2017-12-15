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


		private float _width;
		private float _height;
		private float _maxHeight;
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
			_tileDescriptions = new Dictionary<TILE_TYPE, TileDescription>
			{
				{ TILE_TYPE.NORMAL, TileDescription.Build("Assets/Prefabs/TileDefault.prefab") },
				{ TILE_TYPE.OBSTACLE, TileDescription.Build("Assets/Prefabs/TileDeath.prefab") },
				{ TILE_TYPE.POINT, TileDescription.Build("Assets/Prefabs/TilePoint.prefab") }
			};
		}

		private void NewLine()
		{
			_width = 3;
			_height = _maxHeight;
		}

		private Rect MakeRect(float rectWidth, float rectHeight, bool newLine = false)
		{
			Rect res = new Rect(_width, _height, rectWidth, rectHeight);
			_width += rectWidth + 3;
			_maxHeight = Mathf.Max(_height + rectHeight + 5, _maxHeight);
			if (!newLine)
			{
				return res;
			}
			_width = 3;
			_height = _maxHeight;
			return res;
		}

		public override void OnInspectorGUI()
		{
			_height = 50;
			_maxHeight = 0;
			_width = 3;
			
			if (GUI.Button(MakeRect(150, 17, true), "Refresh"))
			{
				SetDescriptions();
			}
			List<Texture2D> images = new List<Texture2D>();
			foreach (KeyValuePair<TILE_TYPE, TileDescription> kvp in _tileDescriptions)
			{

				GUI.Label(MakeRect(100, 17), kvp.Key.ToString());
				images.Add(kvp.Value.image);

			}
			NewLine();
			foreach (Texture2D im in images)
			{
				GUI.Label(MakeRect(100, 100), im);
			}
			
			NewLine();
			
			if (GUI.Button(MakeRect(200, 17), "Add wave"))
			{
				_data.wavesList.Add(new WaveData());
			}
			if (GUI.Button(MakeRect(200, 17, true), "Save"))
			{
				EditorUtility.SetDirty(_data);
				AssetDatabase.SaveAssets();
			}

			NewLine();
			
			for (int waveIdx = 0; waveIdx < _data.wavesList.Count; ++waveIdx)
			{
				MakeRect(20, 17);
				bool foldout = EditorGUI.Foldout(MakeRect(200, 17), waveIdx == _foldoutId, "Wave " + waveIdx);
				if (GUI.Button(MakeRect(20, 20, true), "X"))
				{
					_data.wavesList.RemoveAt(waveIdx);
					return;
				}

				if (!foldout)
				{
					if (_foldoutId == waveIdx)
					{
						_foldoutId = -1;
					}
					continue;
				}
				_foldoutId = waveIdx;
				WaveData wave = _data.wavesList[waveIdx];
				MakeRect(20, 17);
				wave.width = Mathf.Clamp(EditorGUI.IntField(MakeRect(200, 17, true), wave.width), 2, 10);
				for (int lineIdx = 0; lineIdx < wave.lines.Count; ++lineIdx)
				{
					MakeRect(20, 17);
					string[] tileStringsTab = wave.lines[lineIdx].Split('-');

					List<string> tileStrings = new List<string>();



					for (int tileId = 0; tileId < wave.width; ++tileId)
					{
						TILE_TYPE type = TILE_TYPE.NORMAL;
						if (tileId < tileStringsTab.Length && string.IsNullOrEmpty(tileStringsTab[tileId]) == false)
						{
							type = (TILE_TYPE)Enum.Parse(typeof(TILE_TYPE), tileStringsTab[tileId]);
						}

						tileStrings.Add(type.ToString());

						if (!GUI.Button(MakeRect(100, 100), _tileDescriptions[type].image))
						{
							continue;
						}
						type = (TILE_TYPE)(((int)type + 1) % Enum.GetValues(typeof(TILE_TYPE)).Length);
						tileStrings[tileId] = type.ToString();
					}

					string newString = string.Join("-", tileStrings.ToArray());
					// ReSharper disable once RedundantCheckBeforeAssignment
					if (_data.wavesList[waveIdx].lines[lineIdx] != newString)
					{
						_data.wavesList[waveIdx].lines[lineIdx] = newString;
					}

					NewLine();
				}

				MakeRect(20, 17);
				if (GUI.Button(MakeRect(200, 17, true), "Add Line"))
				{
					wave.lines.Add("");
				}
			}

		}
	}
}