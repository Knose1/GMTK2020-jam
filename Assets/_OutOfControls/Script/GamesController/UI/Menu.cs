using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Github.Knose1.Flow.Engine.Machine.Interfaces;
using Com.Github.Knose1.Flow.Engine.Machine;
using UnityEngine.UI;
using System;

namespace Com.Github.Knose1.OutOfControls.GamesController.UI
{
	public class Menu : MonoBehaviour, IStateStart, IStateEnd
	{
		private Thread thread;
		public Text score;
		public Text timeSurvived;
		public Text bestTime;

		public Button firstStart;
		public Button secondStart;
		public Button thirdStart;
		public Button exit;

		public void OnStart(Thread thread)
		{
			SaveMananger.Data data = SaveMananger.Instance.data;
			score.text = data.limitedTimeModeScore.ToString();
			timeSurvived.text = FormatTime(data.memoryLackModeTime);
			bestTime.text = FormatTime(data.limitedTimeModeScore);

			firstStart.onClick.AddListener(FirstStart_OnClick);
			secondStart.onClick.AddListener(SecondStart_OnClick);
			thirdStart.onClick.AddListener(ThirdStart_OnClick);

			exit.onClick.AddListener(Exit_OnClick);

			this.thread = thread;
		}


		private void FirstStart_OnClick()  => thread.StateMachine.SetTrigger(ScreenManager.FIRST_START_TRIGGER);
		private void SecondStart_OnClick() => thread.StateMachine.SetTrigger(ScreenManager.SECOND_START_TRIGGER);
		private void ThirdStart_OnClick()  => thread.StateMachine.SetTrigger(ScreenManager.THIRD_START_TRIGGER);

		private void Exit_OnClick() => Application.Quit();

		private static string FormatTime(double seconds)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
			string stringSeconds = timeSpan.ToString(@"ss\,fff");
			string stringMin = timeSpan.ToString(@"mm"); 
			string stringHour = timeSpan.ToString(@"hh");

			if (stringHour == "00")
			{
				stringHour = "";
				if (stringMin == "00")
				{
					stringMin = "";
				}
			}

			if (stringHour.Length > 0) stringHour += ":";
			if (stringMin.Length > 0) stringMin += ":";

			return stringHour + stringMin + stringSeconds;
		}

		public void OnEnd(Thread thread)
		{

			firstStart.onClick.RemoveListener(FirstStart_OnClick);
			secondStart.onClick.RemoveListener(SecondStart_OnClick);
			thirdStart.onClick.RemoveListener(ThirdStart_OnClick);
		}
	}
}
