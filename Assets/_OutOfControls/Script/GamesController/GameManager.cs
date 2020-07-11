using Com.Github.Knose1.OutOfControls.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Github.Knose1.OutOfControls.GamesController
{
	

	public class GameManager : MonoBehaviour
	{
		protected float gameCountdown = 0;

		protected float startTimestamp = -1;
		protected PlayerBase currentPlayer;
		protected PlayerBase nextPlayer;
		protected Action doAction;
		protected Action doActionOnLevelManagerReady;

		[SerializeField] protected LevelManager levelManager;

		public void Awake()
		{
			PlayerBase.OnPlayerReady += Player_OnPlayerReady;
			levelManager.OnReady += LevelManager_OnReady;
		}

		private void Start()
		{
			StartGame();
		}

		private void Update()
		{
			doAction?.Invoke();
		}

		public void StartGame()
		{
			levelManager.PreloadNextLevel();
			doActionOnLevelManagerReady = OnGameReadyOnStart;
		}

		private void OnGameReadyOnStart()
		{
			startTimestamp = Time.time;
			doAction = DoActionNormal;
			OnGameReady();
		}

		private void OnGameReady()
		{
			gameCountdown = 10;
			currentPlayer = nextPlayer;
			doActionOnLevelManagerReady = null;
			levelManager.NextLevel();
		}

		private void DoActionNormal()
		{
			if (gameCountdown <= 0 && levelManager.IsReady)
			{
				OnGameReady();
			}
			gameCountdown -= Time.deltaTime;
		}

		public void StopGame()
		{
			doAction = null;
			doActionOnLevelManagerReady = DispatchOnCanCloseGame;

			levelManager.ClosePreloadLevel();
			levelManager.CloseCurrentLevel();

			currentPlayer?.StopMachine();
			nextPlayer?.StopMachine();
		}


		private void DispatchOnCanCloseGame() 
		{
			
		}

		private void LevelManager_OnReady()
		{
			doActionOnLevelManagerReady?.Invoke();
		}

		private void Player_OnPlayerReady(PlayerBase obj)
		{
			if (currentPlayer == null) currentPlayer = obj;
			else nextPlayer = obj;
		}

		private void OnDestroy()
		{
			PlayerBase.OnPlayerReady -= Player_OnPlayerReady;
		}
	}
}

