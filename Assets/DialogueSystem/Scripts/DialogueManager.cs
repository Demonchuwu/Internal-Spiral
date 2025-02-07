// maded by Pedro M Marangon
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CCIJ.Sounds;
using PedroUtils;
using System;
using System.IO.Pipes;

namespace CCIJ.DialogueSystem
{
	public class DialogueManager : MonoBehaviour
	{
		[Header("UI")]
		[SerializeField] private TMP_Text speakerText;
		[SerializeField] private Image speakerImage;
		[SerializeField] private TMP_Text bodyText;
		[SerializeField] private CanvasGroup dialogueUI;
		[Header("Choices")]
		[SerializeField] private Transform choiceButtonParent;
		[SerializeField] private Button choiceButtonPrefab;
		[Header("Audio")]
		[SerializeField] private SFX typeEffect;
		[Range(1,5), SerializeField] private int typeSoundFrequency = 2;
		[Header("Misc")]
		[SerializeField] private float speed = 0.1f;
		private int currentLine = -1;
		private DialogueHolder npc;
		private DialogueSO currentDialogue = null;
		private Coroutine characterCoroutine = null;


		public event Action OnDialogueStarted;
		public event Action<DialogueHolder> OnDialogueFinished;
		public event Action<float> OnChoiceSelected;

		private DialogueSO.DialogueLine CurrentDialogueLine => currentDialogue.GetDialogueLine(currentLine);

		private void Start() => SetDialogueWindowActive(false);

		public void StartDialogue(DialogueSO dialogue, DialogueHolder dialogueHolder)
		{
			OnDialogueStarted?.Invoke();
			npc = dialogueHolder;
			currentDialogue = dialogue;
			currentLine = 0;
			ClearAllChoices();
			SetUI();
			SetDialogueWindowActive(false);
			SetDialogueWindowActive(true);
		}

		public void GoToNextLine()
		{
			currentLine++;
			SetUI(currentLine);
		}

		private void SetDialogueWindowActive(bool active)
		{
			dialogueUI.alpha = active ? 1 : 0;
			dialogueUI.interactable = dialogueUI.blocksRaycasts = active;
		}

		private void SetUI() => SetUI(currentLine);
		private void SetUI(int index)
		{
			if (characterCoroutine != null) return;

			if(index >= currentDialogue.LineCount)
			{
				SetDialogueWindowActive(false);
				currentLine = -1;
				currentDialogue = null;
				OnDialogueFinished?.Invoke(npc);
				npc = null;
				return;
			}

			bodyText.maxVisibleCharacters = 0;


			bodyText.text = CurrentDialogueLine.text;
			speakerText.text = CurrentDialogueLine.speaker.Name;
			speakerImage.sprite = CurrentDialogueLine.speaker.Sprite;
			characterCoroutine = StartCoroutine(nameof(UpdateCharacterCount));
		}

		private IEnumerator UpdateCharacterCount()
		{
			var delay = new WaitForSeconds(speed);
			
			for (int i = 0; i < bodyText.text.Length; i++)
			{
				typeEffect.Play();
				bodyText.maxVisibleCharacters++;
				yield return delay;
			}

			bool hasButtons = CreateButtons();

			if (hasButtons) yield break;

			yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
			
			if(!IsPointOverUI(Input.mousePosition))
			{
				characterCoroutine = null;
				GoToNextLine();
			}
		}

		private void PlayAudio(int charCount)
		{
			if(charCount % typeSoundFrequency == 0)
			typeEffect.Play();
		}

		private bool CreateButtons()
		{
			ClearAllChoices();

			if (currentLine < currentDialogue.LineCount-1) return false;
			bool hasChoices = false;
			foreach (var choice in CurrentDialogueLine.choices)
			{
				Button button = Instantiate(choiceButtonPrefab, choiceButtonParent).GetComponent<Button>();
				button.onClick.AddListener(() =>
				{
					characterCoroutine = null;
					OnChoiceSelected?.Invoke(choice.energyIncrease);
					StartDialogue(choice.continuation, npc);
				});
				button.transform.GetComponentInChildren<TMP_Text>().text = choice.text;
				hasChoices = true;
			}

			return hasChoices;
		}
		private void ClearAllChoices()
		{
			foreach (Transform button in choiceButtonParent) Destroy(button.gameObject);
		}

		private bool IsPointOverUI(Vector3 screenPos)
		{
			bool insideScreen = screenPos.x < Screen.width && screenPos.x > 0 && screenPos.y < Screen.height && screenPos.y > 0;
			if (!insideScreen) return false;

			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.position = screenPos;

			List<RaycastResult> resultList = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, resultList);

			return resultList.Count > 0;
		}
	}
}