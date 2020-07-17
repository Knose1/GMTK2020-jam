using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.GamesController
{
	public class WholeGameManager : MonoBehaviour
	{
		/// <summary>
		/// Returns true when the games end
		/// </summary>
		/// <returns></returns>
		protected delegate bool GameEndCondition();
		public enum GameMode
		{
			limitedTime,
			memoryLack,
			scoreTarget,
		}

		public event Action OnEnd;

		public float GameDuration => endTimestamp - startTimestamp;
		protected float score;

		protected float gameCountdown = 0;
		protected float startTimestamp = -1;
		protected float endTimestamp = -1;
		protected PlayerBase currentPlayer;
		protected PlayerBase nextPlayer;
		protected Action doAction;
		protected GameEndCondition doGameMode;
		protected Action doSaveScore;
		protected Action doActionOnLevelManagerReady;

		[SerializeField] protected Camera camera;
		[SerializeField] protected LevelManager levelManager;
		[SerializeField] protected int gameTime = 20;
		[SerializeField] protected AnimationCurve memoryLack;
		[SerializeField] protected float limitedTime;
		[SerializeField] protected int scoreTarget;
		private bool switchGame;

		public void Awake()
		{
			PlayerBase.OnPlayerReady += Player_OnPlayerReady;
			levelManager.OnReady += LevelManager_OnReady;
		}

		private void Update()
		{
			doAction?.Invoke();
		}

		public void StartGame(GameMode mode)
		{
			levelManager.PreloadNextLevel();
			doActionOnLevelManagerReady = OnGameReadyOnStart;

			switch (mode)
			{
				case GameMode.limitedTime:
					doGameMode = GameModeLimitedTime;
					doSaveScore = SaveLimitedTime;
					break;
				case GameMode.memoryLack:
					score = 100;
					doGameMode = GameModeMemoryLack;
					doSaveScore = SaveMemoryLack;
					break;
				case GameMode.scoreTarget:
					doGameMode = GameModeScoreTarget;
					doSaveScore = SaveScoreTarget;
					break;
			}
		}

		
		private void OnGameReadyOnStart()
		{
			camera.gameObject.SetActive(false);
			startTimestamp = Time.time;
			doAction = DoActionNormal;
			OnGameReady();
			switchGame = false;
		}

		private void OnGameReady()
		{
			gameCountdown = gameTime;
			switchGame = true;

			if (currentPlayer) currentPlayer.OnScore -= CurrentPlayer_OnScore;

			currentPlayer = nextPlayer;

			if (currentPlayer) currentPlayer.OnScore += CurrentPlayer_OnScore;

			doActionOnLevelManagerReady = null;
			levelManager.NextLevel();
		}

		private void DoActionNormal()
		{
			endTimestamp = Time.time;

			if (doGameMode()) 
			{
				StopGame();
				return;
			}

			if (gameCountdown <= 0 && levelManager.IsReady)
			{
				OnGameReady();
			}
			gameCountdown -= Time.deltaTime;
			switchGame = false;
		}

		public void StopGame()
		{
			endTimestamp = Time.time;

			doAction = null;
			doActionOnLevelManagerReady = DispatchOnCanCloseGame;

			levelManager.ClosePreloadLevel();
			levelManager.CloseCurrentLevel();

			score = Mathf.FloorToInt(score);
			doSaveScore();
		}



		private bool GameModeLimitedTime()
		{
			return Time.time - startTimestamp > limitedTime;
		}
		private void SaveLimitedTime()
		{
			if (score > SaveMananger.Instance.data.limitedTimeModeScore)
				SaveMananger.Instance.data.limitedTimeModeScore = score;
		}

		private bool GameModeMemoryLack()
		{
			score -= Time.deltaTime * memoryLack.Evaluate(GameDuration);

			score = Mathf.Max(-10, score);

			return score <= 0;
		}
		
		private void SaveMemoryLack()
		{
			if (GameDuration > SaveMananger.Instance.data.memoryLackModeTime)
				SaveMananger.Instance.data.memoryLackModeTime = GameDuration;
		}

		private bool GameModeScoreTarget()
		{
			return score > scoreTarget;
		}
		
		private void SaveScoreTarget()
		{
			bool isFirstTime =  SaveMananger.Instance.data.currentState != SaveMananger.Data.CurrentState.menu;

			if (GameDuration < SaveMananger.Instance.data.scoreTargetModeTime || isFirstTime)
				SaveMananger.Instance.data.scoreTargetModeTime = GameDuration;
		}



		private void CurrentPlayer_OnScore(float score)
		{
			this.score += score;
		}

		private void DispatchOnCanCloseGame()
		{
			camera.gameObject.SetActive(true);
			OnEnd?.Invoke();
		}

		private void LevelManager_OnReady()
		{
			doActionOnLevelManagerReady?.Invoke();
		}

		private void Player_OnPlayerReady(PlayerBase obj)
		{
			if (!currentPlayer)
			{
				currentPlayer = obj;
				currentPlayer.OnScore += CurrentPlayer_OnScore;
			}
			else nextPlayer = obj;
		}



		private void OnDestroy()
		{
			PlayerBase.OnPlayerReady -= Player_OnPlayerReady;
		}
	}
}

