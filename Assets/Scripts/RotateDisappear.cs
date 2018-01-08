using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RotateDisappear : MonoBehaviour
{
	public IProgress progressObject;
	public List<GameObject> renderers;
	
	void Awake()
	{
		MonoBehaviour[] behaviors =  GetComponents<MonoBehaviour>();

		for (int behaviorIndex = 0; behaviorIndex < behaviors.Length; ++behaviorIndex)
		{
			if (behaviors[behaviorIndex].GetType().GetInterfaces().Contains(typeof(IProgress)))
			{
				progressObject = behaviors[behaviorIndex] as IProgress;
			}
		}
	}
	
	void Update()
	{
		int progress = Mathf.Min(Mathf.CeilToInt(progressObject.GetProgress() * renderers.Count), renderers.Count - 1);

		for (int rendererId = 0; rendererId < renderers.Count; ++rendererId)
		{
			renderers[rendererId].SetActive(renderers[rendererId] == renderers[progress]);
		}
	}
	/*
	protected void Awake()
	{
		StartCoroutine(RotateKilling());
	}

	private IEnumerator RotateKilling()
	{
		yield return new WaitForSeconds(offset);
		while (true)
		{
			yield return new WaitForSeconds(delay / 2.0f);
			visual.enabled = !visual.enabled;
		}
	}*/
}
