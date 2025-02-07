using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
	protected PlayerController myPlayer;

    private void Awake() => myPlayer = FindObjectOfType<PlayerController>();

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (!collision.CompareTag("Player")) return;
        Interact();
    }

    protected abstract void Interact();
}