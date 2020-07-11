

using Com.Github.Knose1.OutOfControls.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.OneButtonGame
{
	public sealed class OBGGameManager : RootManager
	{
		private float countDown;

		public GameObject bulletPrefab;
		public float minWaitTimeBetweenBullet;
		public float maxWaitTimeBetweenBullet;
		public List<Transform> spawners;

		public override void OnStart()
		{
			doAction = SpawnBullet;
		}

		private void SpawnBullet()
		{
			//Instantiate a bullet on any spawner in the spawner list
			Instantiate(bulletPrefab, spawners[Random.Range(0, spawners.Count)]);
			
			countDown = Random.Range(minWaitTimeBetweenBullet, maxWaitTimeBetweenBullet);
			doAction = Wait;
		}

		private void Wait()
		{
			if (countDown <= 0)
			{
				doAction = SpawnBullet;
			}
			countDown -= Time.deltaTime;
		}
	}
}
