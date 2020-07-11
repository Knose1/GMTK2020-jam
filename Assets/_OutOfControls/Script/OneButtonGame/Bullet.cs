using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.OneButtonGame
{
	public class Bullet : CollidesWith
	{
		public float speed = 1;

		public Bullet()
		{
			collides.Add(typeof(Player));
			collides.Add(typeof(PlayerAttack));
		}

		private void Update()
		{
			transform.Translate(Vector3.down * speed * Time.deltaTime);
		}

		public override void OnAnyCollisionEnter(Component component, Type type)
		{
			Destroy(gameObject);
		}
	}
}
