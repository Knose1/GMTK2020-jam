using Com.Github.Knose1.Flow.Engine.Machine;
using Com.Github.Knose1.Flow.Engine.Machine.State;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Com.Github.Knose1.OutOfControls.GamesController
{
	public class SaveMananger
	{
		private const string FILE_PATH = "data.json";
		private static SaveMananger _instance;
		public static SaveMananger Instance => _instance ?? (_instance = new SaveMananger());

		[System.Serializable]
		public struct Data
		{
			[System.Serializable]
			public enum CurrentState
			{
				first,
				second,
				third,
				menu
			}

			public CurrentState currentState;
			public float limitedTimeModeScore;
			public float memoryLackModeTime;
			public float scoreTargetModeTime;
		}

		public Data data;

		public SaveMananger()
		{
			string path = Path.Combine(Application.persistentDataPath, FILE_PATH);
			if (!File.Exists(path))
			{
				File.CreateText(path);
				Debug.Log("Created data at path : "+ path);

				data = default;
				return;
			}

			string text = File.ReadAllText(path);

			Debug.Log("--------------");
			Debug.Log("-----LOAD-----");
			Debug.Log("Path : " + path);
			Debug.Log("Raw Json : " + text);
			Debug.Log("--------------");

			if (text == "") data = default;
			else data = JsonUtility.FromJson<Data>(text);
		}

		public void WriteData()
		{
			string path = Path.Combine(Application.persistentDataPath, FILE_PATH);
			string text = JsonUtility.ToJson(data);
			Debug.Log("---------------");
			Debug.Log("-----WRITE-----");
			Debug.Log("Path : " + path);
			Debug.Log("Raw Json : " + text);
			Debug.Log("---------------");

			File.WriteAllText(path, text);
		}
	}
}

namespace Com.Github.Knose1.OutOfControls.GamesController.UI
{
	public class ScreenManager : StateMachine
	{
		public GameObject firstStartGo;
		public GameObject secondStartGo;
		public GameObject thirdStartGo;
		public GameObject mainMenuGo;

		public const string FIRST_START_TRIGGER = "first";
		public const string SECOND_START_TRIGGER = "second";
		public const string THIRD_START_TRIGGER = "third";
		public const string MENU_START_TRIGGER = "menu";
		public const string NEXT_TRIGGER = "next";

		protected MachineState mainState;
		protected GameObjectMachineState firstStartState;
		protected GameObjectMachineState secondStartState;
		protected GameObjectMachineState thirdStartState;
		protected GameObjectMachineState mainMenuState;
		
		protected MachineState firstGameState;
		protected MachineState secondGameState;
		protected MachineState thirdGameState;

		private void Start()
		{
			StartMachine();
		}

		protected override void SetupMachine()
		{
			base.SetupMachine();

			mainState = new MachineState();

			firstStartState = new GameObjectMachineState(firstStartGo);
			secondStartState = new GameObjectMachineState(secondStartGo);
			thirdStartState = new GameObjectMachineState(thirdStartGo);
			mainMenuState = new GameObjectMachineState(mainMenuGo);

			firstGameState = new MachineState();
			secondGameState = new MachineState();
			thirdGameState = new MachineState();

			AllowTrigger(FIRST_START_TRIGGER);
			AllowTrigger(SECOND_START_TRIGGER);
			AllowTrigger(THIRD_START_TRIGGER);
			AllowTrigger(MENU_START_TRIGGER);
			AllowTrigger(NEXT_TRIGGER);

			mainState.AddTrigger(FIRST_START_TRIGGER, firstStartState);
			mainState.AddTrigger(SECOND_START_TRIGGER, secondStartState);
			mainState.AddTrigger(THIRD_START_TRIGGER, thirdStartState);
			mainState.AddTrigger(MENU_START_TRIGGER, mainMenuState);
			
			firstStartState.AddTrigger(NEXT_TRIGGER, firstGameState);
			secondStartState.AddTrigger(NEXT_TRIGGER, secondGameState);
			thirdStartState.AddTrigger(NEXT_TRIGGER, thirdGameState);

			mainMenuState.AddTrigger(FIRST_START_TRIGGER, firstStartState);
			mainMenuState.AddTrigger(SECOND_START_TRIGGER, secondStartState);
			mainMenuState.AddTrigger(THIRD_START_TRIGGER, thirdStartState);
			
			mainState.OnStart += MainState_OnStart;

			firstGameState.OnStart += FirstGameState_OnStart;
			secondGameState.OnStart += SecondGameState_OnStart;
			thirdGameState.OnStart += ThirdGameState_OnStart;

		}

		private void FirstGameState_OnStart(Thread obj) => StartGame(WholeGameManager.GameMode.limitedTime);
		private void SecondGameState_OnStart(Thread obj) => StartGame(WholeGameManager.GameMode.memoryLack);
		private void ThirdGameState_OnStart(Thread obj) => StartGame(WholeGameManager.GameMode.scoreTarget);

		private void StartGame(WholeGameManager.GameMode mode)
		{
			WholeGameManager game = FindObjectOfType<WholeGameManager>();
			game.StartGame(mode);
			game.OnEnd += Game_OnEnd;
		}

		private void Game_OnEnd()
		{
			StopMachine();

			bool isMenu = SaveMananger.Instance.data.currentState == SaveMananger.Data.CurrentState.menu;
			
			if (!isMenu) SaveMananger.Instance.data.currentState += 1;
			SaveMananger.Instance.WriteData();

			if (isMenu) StartMachine();
			else Application.Quit();
		}

		private void MainState_OnStart(Thread obj)
		{
			switch (SaveMananger.Instance.data.currentState)
			{
				case SaveMananger.Data.CurrentState.first:
					SetTrigger(FIRST_START_TRIGGER);
					break;
				case SaveMananger.Data.CurrentState.second:
					SetTrigger(SECOND_START_TRIGGER);
					break;
				case SaveMananger.Data.CurrentState.third:
					SetTrigger(THIRD_START_TRIGGER);
					break;
				case SaveMananger.Data.CurrentState.menu:
					SetTrigger(MENU_START_TRIGGER);
					break;
			}
		}

		protected override void EntryPoint(Thread mainThread)
		{
			mainThread.SetState(mainState);
		}
	}
}