using Com.Github.Knose1.Common;
using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.Github.Knose1.OutOfControls.GamesController
{
	[System.Serializable]
	public class LevelManager : MonoBehaviour
	{
		public const string SHMUP = "shmup";
		public const string RACE = "race";
		public const string PLATEFORMER = "plateformer";
		public const string TANK = "tank";
		public const string ONE_BUTTON = "oneButton";

		protected int nextLevel = -1;
		protected int currentLevel = -1;

		protected RootManager nextRoot = null;
		protected RootManager currentRoot = null;

		public event Action OnReady;

		[SerializeField] protected int shmupScene = 0;
		[SerializeField] protected int raceScene = 0;
		[SerializeField] protected int plateformerScene = 0;
		[SerializeField] protected int tankScene = 0;
		[SerializeField] protected int oneButtonScene = 0;

		protected List<int> randomScenes;

		public bool IsReady => OperationCount <= 0;
		protected int _operationCount = 0;
		
		protected int OperationCount
		{
			get => _operationCount;
			set
			{
				_operationCount = value;
				if (IsReady) OnReady?.Invoke();
			}
		}

		public void Awake()
		{
			RootManager.OnRootLoaded += Root_OnRootLoaded;
		}

		private void Root_OnRootLoaded(RootManager obj)
		{
			nextRoot = obj;
		}

		private void OnDestroy()
		{
			RootManager.OnRootLoaded -= Root_OnRootLoaded;
		}


		public void NextLevel()
		{
			if (nextLevel != -1)
			{
				CloseCurrentLevel();

				currentRoot = nextRoot;
				currentLevel = nextLevel;
				
				nextRoot = null;
				nextLevel = -1;

				currentRoot?.StartGame();
				PreloadNextLevel();
			}
			else
			{
				Debug.LogWarning("The next level is not preloaded yet");
			}
		}

		public void PreloadNextLevel()
		{
			OperationCount += 1;
			SceneManager.LoadSceneAsync(nextLevel = GetNextRandom(), LoadSceneMode.Additive).completed += SceneActionComplete;
		}

		public void CloseCurrentLevel()
		{
			if (currentLevel == -1) return;
			currentRoot?.StopGame();
			currentRoot = null;

			OperationCount += 1;
			SceneManager.UnloadSceneAsync(currentLevel).completed += SceneActionComplete;
			currentLevel = -1;
		}

		public void ClosePreloadLevel()
		{
			if (nextLevel == -1) return;
			nextRoot = null;

			OperationCount += 1;
			SceneManager.UnloadSceneAsync(nextLevel).completed += SceneActionComplete;
			nextLevel = -1;
		}

		private void SceneActionComplete(AsyncOperation obj)
		{
			OperationCount -= 1;
			obj.completed -= SceneActionComplete;
		}

		protected void GenerateRandomList(int currentScene = -1)
		{
			randomScenes = new List<int>()
			{
				shmupScene,
				raceScene,
				plateformerScene,
				tankScene,
				oneButtonScene
			};

			randomScenes.Shuffle();

			if (currentScene >= 0 && randomScenes.IndexOf(currentScene) < 1)
			{
				randomScenes.Remove(currentScene);
				randomScenes.Insert(UnityEngine.Random.Range(2, randomScenes.Count + 1), currentScene);
			}
		}

		protected int GetNextRandom()
		{
			if (randomScenes == null || randomScenes.Count == 0)
			{
				GenerateRandomList(currentLevel);
			}

			int randomScene = randomScenes[0];
			randomScenes.RemoveAt(0);

			return randomScene;
		}
	}
}
