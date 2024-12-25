using UnityEngine;

public class WeaponPowerPickUp : LootItem
{
    [SerializeField] private AudioData _fullPowerPickUpSFX;

    protected override void PickUp() 
    {
        if (!playerShooting) return;
        
        if (playerShooting.IsFullPower) 
        { 
            pickUpSFX = _fullPowerPickUpSFX;
        }
        else 
        {
            playerShooting.PowerUp();
        }
        
        base.PickUp();
    }
}
