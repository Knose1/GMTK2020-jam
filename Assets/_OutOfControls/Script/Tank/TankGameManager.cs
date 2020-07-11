using UnityEngine;
using Com.Github.Knose1.OutOfControls.Base;
using System.Collections.Generic;
using Com.Github.Knose1.Common;
using System;

namespace Com.Github.Knose1.OutOfControls.Tank
{
	public sealed class TankGameManager : RootManager
	{
		public TankPlayer player;

		[Serializable]
		public struct Patern
		{
			public List<TankTarget> patern;

			public Patern(List<TankTarget> patern) => this.patern = patern;

			public Patern Clone()
			{
				return new Patern(patern.Clone());
			}
		}

		public List<Patern> targetPaterns;
		
		private List<Patern> randomTargetPaterns;
		private Patern currentPatern;

		public override void OnStart()
		{
			GenerateNewRandomList();

			GetNextList();

			base.OnStart();
		}

		private void GetNextList()
		{
			if (randomTargetPaterns.Count == 0)
			{
				GenerateNewRandomList();
			}

			currentPatern = randomTargetPaterns[0];
			randomTargetPaterns.RemoveAt(0);

			foreach (TankTarget item in currentPatern.patern)
			{
				item.OnDie += Target_OnDie;
				item.Activate();
			}
		}

		private void Target_OnDie(TankTarget target)
		{
			target.OnDie -= Target_OnDie;
			currentPatern.patern.Remove(target);
			
			if (currentPatern.patern.Count == 0)
			{
				GetNextList();
			}
		}

		private void GenerateNewRandomList()
		{
			randomTargetPaterns = new List<Patern>();
			int count = targetPaterns.Count;
			for (int i = 0; i < count; i++)
			{
				randomTargetPaterns.Add(targetPaterns[i].Clone());
			}

			randomTargetPaterns = randomTargetPaterns.Shuffle();
		}

		
	}
}
