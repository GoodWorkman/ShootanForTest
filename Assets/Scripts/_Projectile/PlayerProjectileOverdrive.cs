using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] ProjectileGuidanceSystem _guidanceSystem;

    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);
        transform.rotation = Quaternion.identity;  

        if (target == null) base.OnEnable();
        else 
        {
            StartCoroutine(_guidanceSystem.HomingCoroutine(target));
        }
    }
}
