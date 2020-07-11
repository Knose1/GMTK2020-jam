using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Com.Github.Knose1.Common
{
	[RequireComponent(typeof(VideoPlayer), typeof(Animator))]
	public class LoadSceneOnVideoEnd : MonoBehaviour
	{
		public int nextScene = 1;
		public string startTrigger = "start";

		private void Awake()
		{
			GetComponent<VideoPlayer>().loopPointReached += LoadSceneOnVideoEnd_loopPointReached;
		}

		private void LoadSceneOnVideoEnd_loopPointReached(VideoPlayer source)
		{
			source.loopPointReached -= LoadSceneOnVideoEnd_loopPointReached;
			GetComponent<Animator>().SetTrigger(startTrigger);
		}

		public void OnAnimationEnd()
		{
			SceneManager.LoadSceneAsync(nextScene);
		}
	}

}