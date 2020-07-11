using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.Base
{
	public class RootManager : MonoBehaviour
	{
		public static Action<RootManager> OnRootLoaded;

#if UNITY_EDITOR
		[SerializeField] private bool start;
#endif
		[SerializeField] private Camera _camera;
		[SerializeField] private Light _light;
		[SerializeField] private int waitOnStart = 1;


		public Camera Camera => _camera;
		public Light Light => _light;

		protected Action doAction;

		public void Awake()
		{
			gameObject.SetActive(false);
			OnRootLoaded?.Invoke(this);
		}

		private void OnValidate()
		{
			if (start)
			{
				start = false;
				if (Application.isPlaying)
					StartGame();
			}
		}

		public void StartGame()
		{
			gameObject.SetActive(true);
			
			IEnumerator Wait()
			{
				yield return new WaitForSeconds(waitOnStart);
				OnStart();
			}
			
			StartCoroutine(Wait());
		}

		public virtual void OnStart() {}

		public virtual void Update()
		{
			doAction?.Invoke();
		}

		public virtual void StopGame()
		{
			doAction = null;
			gameObject.SetActive(false);
		}
	}
}
