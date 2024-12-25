using UnityEngine;

public class ShieldPickUp : LootItem
{
    [SerializeField] private AudioData _fullHealthPickUpSFX;
    [SerializeField] private float _shieldBonus = 20f;

    protected override void PickUp()
    {
        if (playerCore.IsFullHealth)
        {
            pickUpSFX = _fullHealthPickUpSFX;
        }
        else
        {
            playerCore.RestoreHealth(_shieldBonus);
        }

        base.PickUp();
    }
}