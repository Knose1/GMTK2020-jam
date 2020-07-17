using Com.Github.Knose1.Flow.Engine.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.Base
{
	public abstract class PlayerBase : CollidesWith
	{
		public static event Action<PlayerBase> OnPlayerReady;

		public event Action<float> OnScore;

		protected virtual void Awake()
		{
			OnPlayerReady?.Invoke(this);
		}

		protected void Score(float score)
		{
			OnScore?.Invoke(score);
		}
	}
}
