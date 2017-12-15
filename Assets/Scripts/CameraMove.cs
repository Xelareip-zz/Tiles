using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public Vector3 speed;

	public bool followPlayer;


	private void Awake()
	{
		speed = Vector3.up * Parameters.Parameters.Instance.cameraSpeed;
    }

	private void Update()
	{
		if (followPlayer == false)
		{
			transform.position += speed * Time.deltaTime;
		}
		else
		{
			if (TilePlayer.Instance == null)
			{
				return;
			}
			float cameraOffset = TileCamera.Instance.camera.orthographicSize / 2.0f;
			transform.position += (TilePlayer.Instance.transform.position.y + cameraOffset - transform.position.y) * Time.deltaTime * Vector3.up;
		}
	}
}
