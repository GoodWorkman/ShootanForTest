using System.Collections;
using UnityEngine;

public class PlayerCore : Character
{
    [SerializeField] private StatesBar_HUD _statesBarHUD;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Collider2D _collider2D;

    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerShooting _shooting;
    [SerializeField] private PlayerDodging _dodging;
    
    [SerializeField, Range(0, 1)] private float _healthRegeneratePercent;
    [SerializeField] private bool _regenerateHealth = true;
    [SerializeField] private float _healthRegenerateTime;
    
    [SerializeField] private float _slowMotionDuration = 1f;
    [SerializeField] private float _slomotionTime = 1f;
    
    private Coroutine _healthRegenerateCoroutine;
    private WaitForSeconds _waitForRegenerateTime;
    private WaitForSeconds _waitSlomoTime;
    
    public bool IsFullHealth => health == maxHealth;

    private void Awake()
    {
        _waitForRegenerateTime = new WaitForSeconds(_healthRegenerateTime);
        _waitSlomoTime = new WaitForSeconds(_slomotionTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        _input.onMove += _movement.Move;
        _input.onStopMove += _movement.StopMove;
        _input.onFire += _shooting.StartFiring;
        _input.onStopFire += _shooting.StopFiring;
        _input.onDodge += _dodging.Dodge;
        _input.onLaunchMissle += _shooting.LaunchMissile;
        
        UpdateHUD(health, maxHealth);
    }

    private void OnDisable()
    {
        _input.onMove -= _movement.Move;
        _input.onStopMove -= _movement.StopMove;
        _input.onFire -= _shooting.StartFiring;
        _input.onStopFire -= _shooting.StopFiring;
        _input.onDodge -= _dodging.Dodge;
        _input.onLaunchMissle -= _shooting.LaunchMissile;
    }

    private void Start()
    {
        _input.EnableGameplayInput();
        _statesBarHUD.Initialize(health, maxHealth);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage); 
        TimeController.Instance.BulletTime(_slowMotionDuration);
        _shooting.PowerDown();
        _statesBarHUD.UpdateStates(health, maxHealth);
        
        if (gameObject.activeSelf)
        {
            StartCoroutine(nameof(SlomoCoroutine));

            if (_regenerateHealth)
            {
                if (_healthRegenerateCoroutine != null)
                {
                    StopCoroutine(_healthRegenerateCoroutine);
                }

                _healthRegenerateCoroutine = StartCoroutine(
                    HealthRegenerateCoroutine(_waitForRegenerateTime, _healthRegeneratePercent));
            }
        }
    }
    
    IEnumerator SlomoCoroutine()
    {
        _collider2D.isTrigger = true;
        yield return _waitSlomoTime;
        _collider2D.isTrigger = false;
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value); 
        _statesBarHUD.UpdateStates(health, maxHealth);
    }

    public override void Die()
    {
        GameManager.OnGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        _statesBarHUD.UpdateStates(0, maxHealth);
        base.Die();
    }

    private void UpdateHUD(float currentHealth, float maxHealth)
    {
        _statesBarHUD.UpdateStates(currentHealth, maxHealth);
    }
}
