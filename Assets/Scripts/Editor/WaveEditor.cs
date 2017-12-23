using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[CustomEditor(typeof(WavesData))]
	public class WaveEditor : UnityEditor.Editor
	{
		private static Dictionary<string, Texture2D> _tileImages = new Dictionary<string, Texture2D>();

		private WavesData _data;


		private float _width;
		private float _height;
		private float _maxHeight;
		private int _foldoutId;
		private int _dataWidth;

		private Vector2Int _selectedCoordinates;
		private Texture2D _selectedTexture;

		private static List<GameObject> tiles;

		private TextAsset file;

		private void OnEnable()
		{
			tiles = Resources.LoadAll<GameObject>("Tiles").ToList();
			SetDescriptions();
			_data = (WavesData)target;
			if (_data.wavesList == null)
			{
				_data.wavesList = new List<WaveData>();
				_data.wavesList.Add(new WaveData());
			}
			_dataWidth = _data.wavesList[0].width;
			
			if (_selectedTexture == null)
			{
				_selectedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/Editor/Selected.png");
			}
		}

		private static void SetDescriptions()
		{
			_tileImages.Clear();
			foreach (GameObject tile in tiles)
			{
				_tileImages.Add(tile.name, AssetPreview.GetAssetPreview(tile));
			}
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

		private bool DetectClick(Rect zone)
		{
			if (Event.current != null && Event.current.type == EventType.MouseDown)
			{
				if (zone.Contains(Event.current.mousePosition))
				{
					return true;
				}
			}
			return false;
		}

		public override void OnInspectorGUI()
		{
			_height = 50;
			_maxHeight = 0;
			_width = 3;

			bool typeChanged = false;
			string clickedName = "";
			GUI.Label(MakeRect(150, 17), "File to load");
			file = (TextAsset)EditorGUI.ObjectField(MakeRect(250, 17, true), file, typeof(TextAsset));
			if (GUI.Button(MakeRect(350, 17, true), "Load file"))
			{
				_data.LoadFile(file);
			}
			NewLine();
			if (GUI.Button(MakeRect(150, 17, true), "Refresh"))
			{
				SetDescriptions();
			}
			float previewSize = 75;
			int count = 0;
			List<Texture2D> images = new List<Texture2D>();
			foreach (GameObject tile in tiles)
			{
				++count;
				Rect currentRect =  MakeRect(previewSize, 17); 
				GUI.Label(currentRect, tile.name.Replace("Tile", ""));
				images.Add(AssetPreview.GetAssetPreview(tile));
				
				currentRect.height += previewSize;

				if (DetectClick(currentRect))
				{
					typeChanged = true;
					clickedName = tile.name;
				}
				
				if (count > 5)
				{
					NewLine();
					foreach (Texture2D im in images)
					{
						GUI.Label(MakeRect(previewSize, previewSize), im);
					}
					images.Clear();
					NewLine();
					count = 0;
				}
			}
			NewLine();
			foreach (Texture2D im in images)
			{
				GUI.Label(MakeRect(previewSize, previewSize), im);
			}
			
			NewLine();
			GUI.Label(MakeRect(100, 17), "Width");
			_dataWidth = Mathf.Clamp(EditorGUI.IntField(MakeRect(200, 17, true), _dataWidth), 2, 10);
			
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
				wave.width = _dataWidth;
				for (int lineIdx = wave.lines.Count - 1; lineIdx >= 0; --lineIdx)
				{
					MakeRect(20, 17);
					string[] tileStringsTab = wave.lines[lineIdx].Split('-');

					List<string> tileStrings = new List<string>();

					for (int tileId = 0; tileId < wave.width; ++tileId)
					{
						string name = "TileDefault";
						if (tileStringsTab.Length > tileId)
						{
							name = tileStringsTab[tileId];	
						}

						tileStrings.Add(name);
						Rect selectionRect;
						if (lineIdx == _selectedCoordinates.x && tileId == _selectedCoordinates.y)
						{
							if (typeChanged)
							{
								name = clickedName;
								tileStrings[tileId] = name;	
							}
							
							selectionRect = new Rect(_width - 1, _height - 1, 77, 77);
							EditorGUI.DrawPreviewTexture(selectionRect, _selectedTexture);
						}
						
						Rect currentRect = MakeRect(75, 75);
						if (_tileImages.ContainsKey(name) == false)
						{
							name = "TileDefault";
						}
						GUI.Label(currentRect, _tileImages[name]);
						if (!DetectClick(currentRect))
						{
							continue;
						}
						_selectedCoordinates = new Vector2Int(lineIdx, tileId);
						tileStrings[tileId] = name;
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