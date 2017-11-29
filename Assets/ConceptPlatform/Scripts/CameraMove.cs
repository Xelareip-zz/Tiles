using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public Vector3 speed;

	void Awake()
	{
		speed = Vector3.up * Parameters.Instance.cameraSpeed;
    }

	void Update()
	{
		transform.position += speed * Time.deltaTime;
	}
}
