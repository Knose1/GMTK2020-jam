using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.Tank
{
	public class TankPlayer : PlayerBase
	{
		public string inputAxisX = "MouseX";
		public string inputAxisY = "MouseY";
		public string inputAction = "Action";

		public TankBullet bulletPrefab;
		public Transform canon;
		public Transform turret;
		public Transform bulletSpawner;

		public float bulletInterval = 0.5f;

		public float xSensi = 1;
		public float ySensi = 1;

		public float minXRot = -360;
		public float minYRot = -360;

		public float maxXRot =  360;
		public float maxYRot =  360;

		private float xRot;
		private float yRot;

		private float countDown = 0;

		private void Update()
		{
			if (countDown <= 0 && Input.GetButton(inputAction))
			{
				CreateBullet();
			}

			countDown -= Time.deltaTime;

			xRot += Input.GetAxis(inputAxisX) * Time.deltaTime * xSensi;
			yRot += Input.GetAxis(inputAxisY) * Time.deltaTime * ySensi;

			xRot = xRot % 360;
			yRot = yRot % 360;

			xRot = Mathf.Clamp(xRot, minXRot, maxXRot);
			yRot = Mathf.Clamp(yRot, minYRot, maxYRot);

			turret.localRotation = Quaternion.AngleAxis(xRot, Vector3.up);
			canon.localRotation = Quaternion.AngleAxis(yRot, Vector3.right);

		}

		private void CreateBullet()
		{
			countDown = bulletInterval;
			TankBullet currentBullet = Instantiate(bulletPrefab, bulletSpawner.position, bulletSpawner.rotation, transform.parent);
			currentBullet.OnScore += CurrentBullet_OnScore;
			currentBullet.OnDie += CurrentBullet_OnDie;
		}

		private void CurrentBullet_OnScore()
		{
			Score();
		}

		private void CurrentBullet_OnDie(TankBullet bullet)
		{
			bullet.OnScore -= CurrentBullet_OnScore;
			bullet.OnDie -= CurrentBullet_OnDie;
		}

		public override void OnAnyCollisionEnter(Component component, Type type){}
	}
}
