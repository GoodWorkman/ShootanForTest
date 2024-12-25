using System.Collections;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    [SerializeField] private AudioData _targetAcquiredVoice = null;

    [Header("Speed")]
    [SerializeField] private float _lowSpeed = 8f;
    [SerializeField] private float _highSpeed = 35f;
    [SerializeField] private float _variableSpeedDelay = 0.5f;
    
    [Header("Explosion")]
    [SerializeField] private LayerMask _enemyLayerMask = default;
    [SerializeField] private GameObject _explosionVFX = null;
    [SerializeField] private AudioData _explosionSFX = null;
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private float _explosionDamage = 50f;

    private WaitForSeconds _waitVariableSpeedDelay;

    protected override void Awake() 
    {
        base.Awake();
        _waitVariableSpeedDelay = new WaitForSeconds(_variableSpeedDelay);
    }

    protected override void OnEnable() 
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        
        PoolManager.Release(_explosionVFX, transform.position);
        
        AudioManager.Instance.PlayRandomSfx(_explosionSFX);
        
        var colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayerMask);

        foreach (var collider in colliders) 
        {
            if (collider.TryGetComponent(out Enemy enemy)) {
                enemy.TakeDamage(_explosionDamage);
            }
        }
    }

    IEnumerator VariableSpeedCoroutine() 
    {
        moveSpeed = _lowSpeed;
        yield return _waitVariableSpeedDelay;
        moveSpeed = _highSpeed;

        if (target != null) {
            AudioManager.Instance.PlayRandomSfx(_targetAcquiredVoice);
        }
    }
}
