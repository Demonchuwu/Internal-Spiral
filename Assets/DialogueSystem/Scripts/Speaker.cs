// maded by Pedro M Marangon
using UnityEngine;

namespace CCIJ.DialogueSystem
{
	[CreateAssetMenu(menuName = "Dialogue System/Speaker")]
	public class Speaker : ScriptableObject
	{
		[SerializeField] private Sprite sprite;
		[SerializeField] private string speakerName;

		public Sprite Sprite => sprite;
		public string Name => speakerName;
	}
}