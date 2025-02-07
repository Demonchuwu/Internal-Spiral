// maded by Pedro M Marangon
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCIJ.DialogueSystem
{
	public class DialogueHolder : MonoBehaviour
	{
		[SerializeField] private List<DialogueSO> dialogue;
		private DialogueManager manager;

		public int Count => dialogue.Count;

		private void Awake() => manager = FindObjectOfType<DialogueManager>();

		public void StartDialogue(int index) => manager.StartDialogue(dialogue[index], this);
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(DialogueHolder))]
	public class DialogueHolderEditor : Editor
	{
		private int index;
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (!Application.isPlaying) return;

			DialogueHolder holder = (DialogueHolder)target;

			index = EditorGUILayout.IntSlider(new GUIContent("Dialogue index: "), index, 0, holder.Count - 1);
			if(GUILayout.Button("Start Dialogue"))
			{
				holder.StartDialogue(index);
			}
		}
	}
#endif
}