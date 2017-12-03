using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCamera : MonoBehaviour
{
	private static TileCamera instance;
	public static TileCamera Instance
	{
		get
		{
			return instance;
		}
	}

	new public Camera camera;

	void Awake()
	{
		instance = this;
		float camWidth = Parameters.Instance.width * Parameters.Instance.spaceSize / 2.0f;
        camera.orthographicSize = camWidth / Screen.width * Screen.height;
	}
}
