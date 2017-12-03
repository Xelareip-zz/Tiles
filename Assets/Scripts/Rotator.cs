using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	public float rotSpeed;

	void Update ()
	{
		transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
	}
}
