using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Github.Knose1.OutOfControls.Base;

namespace Com.Github.Knose1.OutOfControls.Race
{
	public class RaceManager : RootManager
	{
		public List<RaceRing> rings;
		private int index = 0;

		public override void OnStart()
		{
			base.OnStart();

			SetRing(0);
		}

		private void SetRing(int index)
		{
			this.index = index;
			rings[index].Activate();
			rings[index].OnCollision += RaceManager_OnCollision;
		}

		private void RaceManager_OnCollision(RaceRing obj)
		{
			obj.OnCollision -= RaceManager_OnCollision;
			SetRing((++index) % rings.Count);
		}
	}
}
