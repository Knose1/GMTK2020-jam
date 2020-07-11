using Com.Github.Knose1.OutOfControls.Base;
using System;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.Tank
{
	public class TankTarget : MonoBehaviour
	{
		public event Action<TankTarget> OnDie;
		public TankTargetCollider targetCollider;

		public Animator animator;
		public string triggerEnd = "end";

		public void Activate()
		{
			gameObject.SetActive(true);
			targetCollider.OnEventAnyCollisionEnter += OnAnyCollisionEnter;
			targetCollider.OnEnd += TargetCollider_OnEnd;
		}

		private void TargetCollider_OnEnd() => gameObject.SetActive(false);

		public void OnAnyCollisionEnter(Component component, Type type)
		{
			OnDie?.Invoke(this);
			animator.SetTrigger(triggerEnd);
		}

		private void OnDestroy()
		{
			OnDie = null;
		}
	}
}
