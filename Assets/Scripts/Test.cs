using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	public List<TemporalPos> lastQueue = new List<TemporalPos>();

	public float retainedAngle;

	void Start()
	{
		SwipeManager.Instance.Swiped += Instance_Swiped;
	}

	private void Instance_Swiped(float obj)
	{
		lastQueue.Clear();
		lastQueue.AddRange(SwipeManager.Instance.inputHistory.ToArray());
		retainedAngle = Mathf.Round(obj / 45.0f) * 45.0f;
	}

	void OnDrawGizmos()
	{
		if (lastQueue.Count > 0)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(lastQueue[0].position), Camera.main.ScreenToWorldPoint(lastQueue[0].position) + Quaternion.AngleAxis(retainedAngle, Vector3.forward) * Vector3.up * 1.5f);
			Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(lastQueue[0].position), Camera.main.ScreenToWorldPoint(lastQueue[0].position) + Quaternion.AngleAxis(retainedAngle - 45.0f, Vector3.forward) * Vector3.up * 1.5f);
			Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(lastQueue[0].position), Camera.main.ScreenToWorldPoint(lastQueue[0].position) + Quaternion.AngleAxis(retainedAngle + 45.0f, Vector3.forward) * Vector3.up * 1.5f);
		}
		Gizmos.color = Color.red;
		foreach (TemporalPos pos in lastQueue)
		{
			Gizmos.DrawWireSphere(Camera.main.ScreenToWorldPoint(pos.position), 0.01f);
		}
	}
}
