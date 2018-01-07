using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEngine;

namespace Parameters
{
	[Serializable]
	[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
	[SuppressMessage("ReSharper", "ConvertToConstant.Global")]
	public class Parameters : ParameterBase
	{
		// ReSharper disable once InconsistentNaming
		private static readonly string SAVE_FILE = Application.persistentDataPath + "/params.json";

		private static Parameters _instance;
		public static Parameters Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}
				_instance = new Parameters();
				_instance.Load();
				return _instance;
			}
		}

		public bool moveNorth = false;
		public bool moveSouth = false;
		public bool moveEast = true;
		public bool moveWest = true;
		public bool moveNorthEast = false;
		public bool moveNorthWest = false;
		public bool moveSouthEast = false;
		public bool moveSouthWest = false;

		[Parameter("Player speed")]
		public float playerSpeed = 4.0f;
		//[Parameter("Camera speed")]
		public float cameraSpeed = 1.0f;

		//[Parameter("Input queue")]
		public int inputQueueSize = 2;
		//[Parameter]
		public bool drawPath = false;

		[Parameter("Distance points")]
		public bool pointsPerLine = true;

		[Parameter]
		public bool loopLeftRight = true;

		[Parameter("Difficulty Increase (%/line)")]
		public float difficultyIncrease = 1.0f;

		//[Parameter]
		public float spaceSize = 1.5f;
		//[Parameter("Swipe?")]
		public bool swipeControl = true;
		[Parameter("Swipe sensibility")]
		public float swipeSensibility = 80.0f;

		[Parameter("Auto move delay")]
		public float autoMoveDelay = 2.0f;
		[ResourceParameter("Levels", "Levels")]
		public string levels = "WavesBasic";


		private Parameters()
		{
		}

		public override void Load()
		{
			if (!File.Exists(SAVE_FILE))
			{
				return;
			}
			string content = File.ReadAllText(SAVE_FILE);
			JsonUtility.FromJsonOverwrite(content, this);
		}

		public override void Save()
		{
			File.WriteAllText(SAVE_FILE, JsonUtility.ToJson(this));
		}

		public override void Reset()
		{
			File.Delete(SAVE_FILE);
		}

		public override void DeleteInstance()
		{
			_instance = null;
		}
	}
}
