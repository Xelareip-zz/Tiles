using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[Serializable]
public class Parameters : ParameterBase
{
	private static string SAVE_FILE = Application.persistentDataPath + "/params.json";

	private static Parameters instance;
	public static Parameters Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Parameters();
				instance.Load();
			}
			return instance;
		}
	}

	[Parameter]
	public bool moveNorth = false;
	[Parameter]
	public bool moveSouth = true;
	[Parameter]
	public bool moveEast = false;
	[Parameter]
	public bool moveWest = false;
	[Parameter]
	public bool moveNorthEast = true;
	[Parameter]
	public bool moveNorthWest = true;
	[Parameter]
	public bool moveSouthEast = false;
	[Parameter]
	public bool moveSouthWest = false;

	[Parameter("Player speed")]
	public float playerSpeed = 4.0f;
	[Parameter("Camera speed")]
	public float cameraSpeed = 1.0f;

	[Parameter("Input queue")]
	public int inputQueueSize = -1;
	[Parameter]
	public bool drawPath = true;

	[Parameter("Distance points")]
	public bool pointsPerLine = true;

	[Parameter]
	public bool loopLeftRight = true;

	[Parameter]
	public bool mustTiles = false;

	[Parameter("Difficulty Increase (%/line)")]
	public float difficultyIncrease = 1.0f;

	[Parameter]
	public int width = 5;
	[Parameter]
	public float spaceSize = 2.0f;
	[Parameter("Death level (height%)")]
	public float deathHeight = 25.0f;



	private Parameters()
	{
	}

	public override void Load()
	{
		if (File.Exists(SAVE_FILE))
		{
			string content = File.ReadAllText(SAVE_FILE);
			JsonUtility.FromJsonOverwrite(content, this);
		}
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
		instance = null;
	}
}
