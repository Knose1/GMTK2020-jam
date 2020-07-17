using Com.Github.Knose1.Flow.Engine.Machine;
using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.Race
{
	[RequireComponent(typeof(Rigidbody))]
	public class RacePlayer : PlayerBase
	{
		public RacePlayer()
		{
			collides.Add(typeof(RaceRing));
		}

		public string horizontal = "Horizontal";
		public string vertical = "Vertical";
		
		public float acceleration = 0;
		public float friction = 0;
		public float minSpeed = -3;
		public float maxSpeed = 10;

		protected float speed = 0;

		protected new Rigidbody rigidbody;
		private float y = 0;

		protected override void Awake()
		{
			base.Awake();
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			speed /= friction;

			speed += acceleration * Input.GetAxis(vertical);
			y += Input.GetAxis(horizontal);

			speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
			
			rigidbody.velocity = transform.forward * speed;

			rigidbody.MoveRotation(Quaternion.AngleAxis(y, Vector3.up));
		}

		public override void OnAnyCollisionEnter(Component component, Type type)
		{
			Score(20);
		}
	}
}
