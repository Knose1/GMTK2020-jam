using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.OneButtonGame
{
	public class OBGPlayerAttack : CollidesWith
	{
		public enum Direction
		{
			Left = 0,
			Right = 1
		}

		public event Action OnScore;
		public event Action<Direction> OnAttack;

		[SerializeField] public float waitDuration = 1;
		[SerializeField] public float attackDuration = 1;
		[SerializeField] public Direction direction;
		[SerializeField] new public Collider2D collider;
		private float attackCountdown = 0;
		private float waitCountdown = 0;
		


		public OBGPlayerAttack()
		{
			collides.Add(typeof(OBGBullet));
		}

		public override void OnAnyCollisionEnter(Component component, Type type)
		{
			OnScore?.Invoke();
		}

		public void Activate()
		{
			collider.enabled = false;
			waitCountdown = waitDuration;
			attackCountdown = attackDuration;
			OnAttack?.Invoke(direction);
		}

		public void Deactivate()
		{
			collider.enabled = false;
			waitCountdown = 0;
			attackCountdown = 0;
		}

		private void Update()
		{
			if (collider.enabled = (attackCountdown > 0 && waitCountdown <= 0))
			{

				attackCountdown -= Time.deltaTime;
			}

			waitCountdown -= Time.deltaTime;
		}
	}
}
