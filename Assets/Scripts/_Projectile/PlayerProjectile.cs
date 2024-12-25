using UnityEngine;

public class PlayerProjectile : Projectile
{
    private TrailRenderer _trail;

    protected virtual void Awake() 
    {
        _trail = GetComponentInChildren<TrailRenderer>();
        
        if (moveDirection != Vector2.right) 
        {
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, moveDirection);
        }
    }

    private void OnDisable() 
    {
        _trail.Clear();  
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        PlayerEnergy.Instance.Obtain(PlayerEnergy.PERCENT);  
    }

}
