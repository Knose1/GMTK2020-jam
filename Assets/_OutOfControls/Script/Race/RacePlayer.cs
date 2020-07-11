using Com.Github.Knose1.Flow.Engine.Machine;
using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.Race
{
	public class RacePlayer : PlayerBase
	{
		public override void OnAnyCollisionEnter(Component component, Type type) => throw new NotImplementedException();
	}
}
