using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public Vector3 speed;

	public bool followPlayer = false;


	void Awake()
	{
		speed = Vector3.up * Parameters.Instance.cameraSpeed;
    }

	void Update()
	{
		if (followPlayer == false)
		{
			transform.position += speed * Time.deltaTime;
		}
		else
		{
			if (TilePlayer.Instance != null)
			{
				transform.position += (TilePlayer.Instance.transform.position.y - transform.position.y) * Time.deltaTime * Vector3.up;
			}
		}
	}
}
