using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Github.Knose1.Common
{
	public class UpAlwaysUp : MonoBehaviour
	{
		protected void Update()
		{
			transform.rotation = Quaternion.LookRotation(transform.parent.forward, Vector3.up);
		}
	}
}
