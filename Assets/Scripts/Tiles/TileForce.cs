using System;
using UnityEngine;


public class TileForce : TileBase
{
	public DIRECTIONS direction;
	public GameObject visual;

	private void Start()
	{
		visual.transform.rotation = Quaternion.AngleAxis(45.0f * (int)direction, Vector3.forward);
	}

	public override void AddOptions(string[] fullString)
	{
		base.AddOptions(fullString);

		string[] directions = Enum.GetNames(typeof(DIRECTIONS));

		bool shouldBreak = false;
		for (int optIndex = 0; optIndex < fullString.Length; ++optIndex)
		{
			for (int dirIndex = 0; dirIndex < directions.Length; ++dirIndex)
			{
				if (directions[dirIndex].ToLower() == fullString[optIndex].ToLower())
				{
					direction = (DIRECTIONS)Enum.Parse(typeof(DIRECTIONS), directions[dirIndex]);
					shouldBreak = true;
					break;
				}
			}

			if (shouldBreak)
			{
				break;
			}
		}
		
		visual.transform.rotation = Quaternion.AngleAxis(45.0f * (int)direction, Vector3.forward);
	}

	public override void TileReached()
	{
		TilePlayer.Instance.ForceTile(neighbors[(int) direction]);
		
		base.TileReached();
	}
}
