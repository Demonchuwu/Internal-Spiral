using PedroUtils;
using UnityEngine;

public class PotionCollect : Collectable
{
    [SerializeField] private float healAmount = 2;
    protected override void Interact()
    {
        this.Log("PLAYER");
        myPlayer.PlayDrinkSFX();
        myPlayer.HealthDown(-healAmount, -10);
        Destroy(gameObject);
    }

}
