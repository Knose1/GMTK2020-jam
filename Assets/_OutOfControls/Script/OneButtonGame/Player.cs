using Com.Github.Knose1.Flow.Engine.Machine;
using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.OneButtonGame
{
	public class Player : PlayerBase
	{
		public List<PlayerAttack> playerAttacks;

		public event Action OnScore;

		protected override void Awake()
		{
			base.Awake();
			for (int i = 0; i < playerAttacks.Count; i++)
			{
				playerAttacks[i].OnScore += Player_OnScore;
			}
		}

		private void Player_OnScore()
		{
			OnScore?.Invoke();
		}

		public Player()
		{
			collides.Add(typeof(Bullet));
		}

		public override void OnAnyCollisionEnter(Component component, Type type) {}
	}
}
