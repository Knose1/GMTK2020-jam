using Com.Github.Knose1.Flow.Engine.Machine;
using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.OneButtonGame
{
	[RequireComponent(typeof(Animator))]
	public class OBGPlayer : PlayerBase
	{
		public List<OBGPlayerAttack> playerAttacks;
		protected Animator animator;

		[SerializeField] protected string attackTrigger;
		[SerializeField] protected string hurtTrigger;
		[SerializeField] protected string directionFloat;

		private float targetDirection;
		private float direction;

		protected override void Awake()
		{
			base.Awake();
			animator = GetComponent<Animator>();
			for (int i = 0; i < playerAttacks.Count; i++)
			{
				playerAttacks[i].OnScore += Player_OnScore;
				playerAttacks[i].OnAttack += Player_OnAttack; ;
			}
		}

		private void Player_OnAttack(OBGPlayerAttack.Direction direction)
		{
			animator.SetTrigger(attackTrigger);

			this.targetDirection = (int)direction;
		}

		private void Player_OnScore()
		{
			Score();
		}

		public OBGPlayer()
		{
			collides.Add(typeof(OBGBullet));
		}

		public override void OnAnyCollisionEnter(Component component, Type type) 
		{
			animator.SetTrigger(hurtTrigger);
			for (int i = 0; i < playerAttacks.Count; i++)
			{
				playerAttacks[i].Deactivate();
			}
		}

		private void Update()
		{
			direction = Mathf.Lerp(direction, targetDirection, 0.1f);
			animator.SetFloat(directionFloat, direction);
		}
	}
}
