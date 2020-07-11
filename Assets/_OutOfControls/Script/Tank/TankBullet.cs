using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.Tank
{
	public class TankBullet : CollidesWith
	{
		public event Action OnScore;
		public event Action<TankBullet> OnDie;

		public float speed = 10;

		private Rigidbody rb;

		public TankBullet()
		{
			collides.Add(typeof(TankTargetCollider));
		}

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();
			rb.velocity = transform.forward * speed;

		}

		private void Update()
		{
			if (transform.position.sqrMagnitude >= 20000)
			{
				OnDie?.Invoke(this);
				Destroy(gameObject);
			}
		}

		public override void OnAnyCollisionEnter(Component component, Type type)
		{
			OnScore?.Invoke();
			OnDie?.Invoke(this);
			Destroy(gameObject);
		}

		private void OnDestroy()
		{
			OnScore = null;
			OnDie = null;
		}
	}
}
