public class MisslePickUp : LootItem
{
    protected override void PickUp()
    {
        playerShooting.PickUpMissile();
        base.PickUp();
    }
}