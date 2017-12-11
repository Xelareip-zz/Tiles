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
	[Parameter("Fractured tiles")]
	public bool fragileTiles = false;

	[Parameter("Difficulty Increase (%/line)")]
	public float difficultyIncrease = 1.0f;

	[Parameter]
	public int width = 5;
	[Parameter]
	public float spaceSize = 2.0f;
	[Parameter("Death level (height%)")]
	public float deathHeight = -50.0f;
	[Parameter("Swipe?")]
	public bool swipeControl = true;
	[Parameter("Swipe sensibility")]
	public float swipeSensibility = 80.0f;

	public bool autoMove = true;

	[Parameter("Auto move delay")]
	public float autoMoveDelay = 2.0f;

	[Parameter]
	public float cameraOffset = 4.0f;



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
