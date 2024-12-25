using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class MissleSystem : MonoBehaviour
{
    [SerializeField] private int _defaultamount = 5;
    [SerializeField] private float _cooldownTime = 1f;
    [SerializeField] private GameObject _misslePrefab = null;
    [SerializeField] private AudioData _launchSFX = null;
    [SerializeField] private MissileDisplay _missileDisplay;
    
    private int _amount;
    private bool _isReady = true;
    private WaitForSeconds _waitForMissleCooldown;

    private void Awake() 
    {
        _amount = _defaultamount;
        _waitForMissleCooldown = new WaitForSeconds(_cooldownTime);
    }

    private void Start() 
    {
        _missileDisplay.UpdateAmountText(_amount);
    }

    public void PickUp() 
    {
        _amount++;
        _missileDisplay.UpdateAmountText(_amount);
        
        if (_amount == 1) {
            _missileDisplay.UpdateCooldownImage(0f);
            _isReady = true;
        }
    }

    public void Launch(Transform muzzleTransform) 
    {
        if (_amount == 0 || !_isReady) return;
        _isReady = false;
        
        PoolManager.Release(_misslePrefab, muzzleTransform.position);
        
        AudioManager.Instance.PlayRandomSfx(_launchSFX);
        _amount--;
        
        _missileDisplay.UpdateAmountText(_amount);

        if (_amount == 0) {
            _missileDisplay.UpdateCooldownImage(1f);
        }
        else {
            StartCoroutine(CooldownCoroutine());
        }
    }

    IEnumerator CooldownCoroutine() 
    {
        float cooldownValue = _cooldownTime;

        while (cooldownValue > 0f) {
            _missileDisplay.UpdateCooldownImage(cooldownValue / _cooldownTime);
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);

            yield return null;  
        }     

        _isReady = true;
    }
}
