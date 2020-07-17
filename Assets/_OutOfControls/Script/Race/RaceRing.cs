using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.Race
{
	public class RaceRing : CollidesWith
	{
		public event Action<RaceRing> OnCollision;

		public RaceRing()
		{
			collides.Add(typeof(RacePlayer));
		}

		public void Activate()
		{
			gameObject.SetActive(true);
		}

		public override void OnAnyCollisionEnter(Component component, Type type)
		{
			OnCollision?.Invoke(this);
			gameObject.SetActive(false);
		}
	}
}
