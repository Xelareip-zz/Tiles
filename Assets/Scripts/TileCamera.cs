using UnityEngine;

public class TileCamera : MonoBehaviour
{
	public static TileCamera Instance { get; private set; }

	public new Camera camera;

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		float camWidth = TileLine.lineWidth * Parameters.Parameters.Instance.spaceSize / 2.0f;
		camera.orthographicSize = camWidth / Screen.width * Screen.height;
	}
}
