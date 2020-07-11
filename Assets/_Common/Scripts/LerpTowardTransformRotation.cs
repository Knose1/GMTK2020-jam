using UnityEngine;

namespace Com.Github.Knose1.Common
{
	public class LerpTowardTransformRotation : MonoBehaviour
	{
		public Transform target;
		public float minSpeed = 1;
		public float maxSpeed = 1;
		public float speedAcceleration = 1;
		private float speed = 1;

		private Quaternion targetRot = default; 

		public void Update()
		{
			Quaternion rotation = target.rotation;
			if (transform.rotation == rotation)
			{
				speed = minSpeed;
			}
			else
			{
				speed = Mathf.Min(maxSpeed, speed + speedAcceleration);
			}
			targetRot = rotation;

			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, speed * Time.deltaTime);
		}
	}
}
