using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.OneButtonGame
{
	public class OBGBullet : CollidesWith
	{
		public float speed = 1;

		public OBGBullet()
		{
			collides.Add(typeof(OBGPlayer));
			collides.Add(typeof(OBGPlayerAttack));
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
