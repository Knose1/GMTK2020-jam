using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.Base
{
	public abstract class CollidesWith : MonoBehaviour
	{
		protected List<Type> collides = new List<Type>();

		private void OnTriggerEnter(Collider other)
		{
			int count = collides.Count;
			for (int i = 0; i < count; i++)
			{
				Type type = collides[i];
				Component component = other.GetComponent(type);
				if (component)
				{
					OnAnyCollisionEnter(component, type);
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			int count = collides.Count;
			for (int i = 0; i < count; i++)
			{
				Type type = collides[i];
				Component component = other.GetComponent(type);
				if (component)
				{
					OnAnyCollisionEnter(component, type);
				}
			}
		}

		public abstract void OnAnyCollisionEnter(Component component, Type type);
	}
}
