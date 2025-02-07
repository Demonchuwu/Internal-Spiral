// Maded by Pedro M Marangon
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class TriggerAction : MonoBehaviour
{
	[Header("Interacting")]
	[SerializeField] private UnityEvent whatToDo = null;
	[SerializeField] private UnityEvent onEnterTrigger = null;
	[SerializeField] private UnityEvent onExitTrigger = null;
	[SerializeField] private bool needsInput = true;
	[SerializeField] private bool onlyInteractOnce = true;
	[Header("Gizmos")]
	[SerializeField] private bool displayGizmos = true;
	[SerializeField] private bool showOnlyWhenSelected = false;
	[SerializeField] private Color gizmosColor;
	private Collider2D _collider;
	private bool isPlayerWithin = false;
	private bool hasBeenInvoked = false;

	private void Reset()
	{
		gizmosColor = Random.ColorHSV();
		gizmosColor.a = 0.75f;
	}

	private void Awake()
	{
		_collider = GetComponent<Collider2D>();
		_collider.isTrigger = true;
		// onExitTrigger?.Invoke();
	}

	private void Update()
	{
		if (!isPlayerWithin || !Input.GetKeyDown(KeyCode.E)) return;
		Interact();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (onlyInteractOnce && hasBeenInvoked) return;
		onEnterTrigger?.Invoke();
		if(other.CompareTag("Player"))
		{
			if (!needsInput) Interact();

			else isPlayerWithin = true;
		}
	}

	private void Interact()
	{
		whatToDo?.Invoke();
		hasBeenInvoked = true;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		onExitTrigger?.Invoke();
		isPlayerWithin = false;
	}

	private void OnDrawGizmos() => DrawGizmos(true);
	private void OnDrawGizmosSelected() => DrawGizmos(false);

	private void DrawGizmos(bool show)
	{
		if (!displayGizmos || showOnlyWhenSelected == show) return;
		if (!_collider) _collider = GetComponent<Collider2D>();
		Gizmos.color = gizmosColor;
		Gizmos.DrawCube(transform.position, _collider.bounds.size);
	}
}