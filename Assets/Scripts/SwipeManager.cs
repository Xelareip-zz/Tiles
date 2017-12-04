using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TemporalPos
{
	public Vector3 position;
	public float time;

	public TemporalPos(Vector3 pos, float t)
	{
		position = pos;
		time = t;
	}
}

public class SwipeManager : MonoBehaviour
{
	private static SwipeManager instance;
	public static SwipeManager Instance
	{
		get
		{
			return instance;
		}
	}

	public event System.Action<float> Swiped;

	public float minDistance;
	public float maxDuration = 1.0f;


	public Queue<TemporalPos> inputHistory = new Queue<TemporalPos>();

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		minDistance = Parameters.Instance.swipeSensibility;
	}

	bool HasInput()
	{
		return Input.GetMouseButton(0) || Input.touchCount != 0;
	}

	Vector3 InputPos()
	{
		if (Input.touchCount != 0)
		{
			return Input.GetTouch(0).position;
		}
		return Input.mousePosition;
	}

	void Update()
	{
		HandleSwipe();
	}

	private void HandleSwipe()
	{
		while (inputHistory.Count > 0)
		{
			TemporalPos input = inputHistory.Peek();
			if (Time.time - input.time > maxDuration)
			{
				inputHistory.Dequeue();
			}
			else
			{
				break;
			}
        }
		if (HasInput())
		{
			inputHistory.Enqueue(new TemporalPos(InputPos(), Time.time));
		}
		else
		{
			if (inputHistory.Count > 0)
			{
				Vector3 currentInput = InputPos();
				TemporalPos startInput = inputHistory.Peek();

				if (Vector3.Distance(currentInput, startInput.position) > minDistance)
				{
					float angle = Quaternion.FromToRotation(Vector3.up, currentInput - startInput.position).eulerAngles.z;
					if (Swiped != null)
					{
						Swiped(angle);
					}
				}
			}
			inputHistory.Clear();
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		foreach(TemporalPos pos in inputHistory.ToArray())
		{
			Gizmos.DrawWireSphere(Camera.main.ScreenToWorldPoint(pos.position), 0.01f);
		}
	}
}
