using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.Base
{
	public sealed class Root : MonoBehaviour
	{
		public static Action<Root> OnRootLoaded;

		public void Awake()
		{
			gameObject.SetActive(false);
			OnRootLoaded?.Invoke(this);
		}
	}
}
