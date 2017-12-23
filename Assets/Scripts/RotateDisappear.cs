using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDisappear : MonoBehaviour
{
	public Renderer visual;

	public float offset;
	public float delay;
	
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
	}
}
