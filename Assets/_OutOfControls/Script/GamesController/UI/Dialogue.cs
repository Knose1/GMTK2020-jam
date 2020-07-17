using Com.Github.Knose1.Flow.Engine.Machine;
using Com.Github.Knose1.Flow.Engine.Machine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Github.Knose1.OutOfControls.GamesController.UI
{
	public class Dialogue : MonoBehaviour, IState, IStateStart, IStateEnd, IStateUpdate
	{
		[Serializable]
		public struct DialogueLine
		{
			[SerializeField, TextArea] public string text;
		}

		private Thread thread;
		private int currentDialogueIndex = 0;
		private int currentTextIndex = 0;
		private float characterCountdown;

		public float timeByCharacter;
		public Button skipButton;
		public Button background;
		public Text text;
		public List<DialogueLine> dialogue;


		public void OnStart(Thread thread)
		{
			text.text = "";
			this.thread = thread;
			background.onClick.AddListener(Background_OnClick);
			skipButton.onClick.AddListener(Skip_OnClick);
		}

		private void Background_OnClick()
		{
			characterCountdown = 0;
			if (text.text.Length < dialogue[currentDialogueIndex].text.Length)
			{
				currentTextIndex = dialogue[currentDialogueIndex].text.Length - 1;
			}
			else
			{
				if (dialogue.Count - 1 == currentDialogueIndex)
				{
					thread.StateMachine.SetTrigger(ScreenManager.NEXT_TRIGGER);
					return;
				}

				currentTextIndex = 0;
				text.text = "";
				currentDialogueIndex += 1;
			}
		}

		private void Skip_OnClick()
		{
			thread.StateMachine.SetTrigger(ScreenManager.NEXT_TRIGGER);
		}

		public void OnEnd(Thread thread)
		{
			background.onClick.RemoveListener(Background_OnClick);
			skipButton.onClick.RemoveListener(Skip_OnClick);
		}

		public void OnUpdate(Thread thread)
		{
			if (characterCountdown <= 0 && text.text.Length < dialogue[currentDialogueIndex].text.Length)
			{
				characterCountdown = timeByCharacter;
				currentTextIndex += 1;
				text.text = dialogue[currentDialogueIndex].text.Substring(0, currentTextIndex);
			}

			characterCountdown -= Time.deltaTime;

		}
	}
}
