using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.Tank
{
	public class TankTargetCollider : CollidesWith
	{
		public event Action<Component,Type> OnEventAnyCollisionEnter;
		public event Action OnEnd;

		public TankTargetCollider() : base()
		{
			collides.Add(typeof(TankBullet));
		}

		public override void OnAnyCollisionEnter(Component component, Type type)
		{
			OnEventAnyCollisionEnter?.Invoke(component, type);
		}

		public void AnimationEnd()
		{
			OnEnd?.Invoke();
		}
	}
}
