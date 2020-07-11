using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Github.Knose1.Common
{
	public class CopyTransform : MonoBehaviour
	{
		public Transform target;

		public void Update()
		{
			transform.SetPositionAndRotation(target.position, target.rotation);
		}
	}
}
