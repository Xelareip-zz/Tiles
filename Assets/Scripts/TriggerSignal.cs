using System;
using UnityEngine;

namespace Common.Scripts
{
	public class TriggerSignal : MonoBehaviour
	{
		
		// ReSharper disable EventNeverSubscribedTo.Global
		public event Action<TriggerSignal, Collider> collisionStay;
		public event Action<Collider> collisionEnter;
		public event Action<Collider> collisionExit;

		public event Action<TriggerSignal, Collider2D> collisionStay2D;
		public event Action<Collider2D> collisionEnter2D;
		public event Action<Collider2D> collisionExit2D;
		// ReSharper restore EventNeverSubscribedTo.Global

		private void OnTriggerEnter(Collider coll)
		{
			if (collisionEnter != null)
			{
				collisionEnter(coll);
			}
		}

		private void OnTriggerStay(Collider coll)
		{
			if (collisionStay != null)
			{
				collisionStay(this, coll);
			}
		}

		private void OnTriggerExit(Collider coll)
		{
			if (collisionExit != null)
			{
				collisionExit(coll);
			}
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (collisionEnter2D != null)
			{
				collisionEnter2D(coll);
			}
		}

		private void OnTriggerStay2D(Collider2D coll)
		{
			if (collisionStay2D != null)
			{
				collisionStay2D(this, coll);
			}
		}

		private void OnTriggerExit2D(Collider2D coll)
		{
			if (collisionExit2D != null)
			{
				collisionExit2D(coll);
			}
		}
	}
}
