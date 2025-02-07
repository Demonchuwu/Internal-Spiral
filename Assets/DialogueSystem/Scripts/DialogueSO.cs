// maded by Pedro M Marangon
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CCIJ.DialogueSystem
{
	[CreateAssetMenu(menuName = "Dialogue System/Dialogue")]
	public class DialogueSO : ScriptableObject
	{
		[SerializeField] private List<DialogueLine> lines = new();

		public int LineCount => lines.Count;

		public DialogueLine GetDialogueLine(int index) => lines[index];

		[Serializable]
		public class DialogueLine
		{
			public Speaker speaker;
			[TextArea(5,10)] public string text;
			public List<Choice> choices;
		}

		[Serializable]
		public class Choice
		{
			public string text;
			public DialogueSO continuation;
			public float energyIncrease = 0;
		}
	}
}