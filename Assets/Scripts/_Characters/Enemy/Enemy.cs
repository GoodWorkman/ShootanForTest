using UnityEngine;

[RequireComponent(typeof(LootSpawner))]
public class Enemy : Character
{
    [SerializeField] private int _deathEnergyBouns = 3;
    [SerializeField] protected int healthFactor;

    LootSpawner lootSpawner;

    protected virtual void Awake() 
    {
        lootSpawner = GetComponent<LootSpawner>();
    }

    protected override void OnEnable() 
    {
        SetHealth();
        base.OnEnable();
    }

    protected virtual void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.TryGetComponent(out PlayerCore player)) {
            Die();
            player.Die();
        }
    }
    
    public override void Die()
    {
        PlayerEnergy.Instance.Obtain(_deathEnergyBouns);
        EnemyManager.Instance.RemoveFromList(gameObject);
        lootSpawner.Spawn(transform.position); 
        base.Die();
    }
    
    private void SetHealth() 
    {
        maxHealth += (int)(EnemyManager.Instance.WaveNumber / healthFactor);
    }
}
