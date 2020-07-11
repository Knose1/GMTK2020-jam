using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.OneButtonGame
{
	public class PlayerAttack : CollidesWith
	{
		public event Action OnScore;

		public PlayerAttack()
		{
			collides.Add(typeof(Bullet));
		}

		public override void OnAnyCollisionEnter(Component component, Type type)
		{
			OnScore?.Invoke();
		}
	}
}
