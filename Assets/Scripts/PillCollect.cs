using PedroUtils;
using System.Collections;
using UnityEngine;

public class PillCollect : Collectable
{
	[SerializeField] private float speedIncrease = 2f;
	[SerializeField] private float effectTime = 2f;

    protected override void Interact()
    {
		this.Log("PLAYER");
		myPlayer.PlayPillSFX();
		myPlayer.IncreaseSpeed(speedIncrease);
		this.Log("Collided pill");
		StartCoroutine("TimeCount");

		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
	}

    private IEnumerator TimeCount()
    {
        this.Log("MOVED 1");
        yield return new WaitForSeconds(effectTime);
        this.Log("MOVED 2");
        myPlayer.IncreaseSpeed(-speedIncrease);
		Destroy(gameObject);
	}
}
