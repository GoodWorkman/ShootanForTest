using System;
using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private MissleSystem _missleSystem;
    
    [SerializeField] private GameObject _projectile1;
    [SerializeField] private GameObject _projectile2;
    [SerializeField] private GameObject _projectile3;
    
    [SerializeField] private Transform _muzzleTop;
    [SerializeField] private Transform _muzzleMiddle;
    [SerializeField] private Transform _muzzleBottom;
    [SerializeField] private ParticleSystem _muzzleVFX;
    
    [SerializeField, Range(0, 2)] private int _weaponPower;
    [SerializeField] private float _fireInterval = 0.2f;
    
    [SerializeField] private AudioData projectileSFXData;
    
    private WaitForSeconds waitForfireInterval;

    private Coroutine _fireCoroutine;

    public bool IsFullPower => _weaponPower == 2;
    
    private void Awake()
    {
        waitForfireInterval = new WaitForSeconds(_fireInterval);
    }

    public void StartFiring()
    {
        if (_fireCoroutine != null)
            return;

        _muzzleVFX.Play();
        _fireCoroutine = StartCoroutine(FireCoroutine());
    }

    public void StopFiring()
    {
        if (_fireCoroutine == null)
            return;

        StopCoroutine(_fireCoroutine);
        _fireCoroutine = null;
        _muzzleVFX.Stop();
    }
    
    public void PowerUp()
    {
        _weaponPower = Mathf.Clamp(++_weaponPower, 0, 2);
    }

    public void PowerDown()
    {
        _weaponPower = Mathf.Clamp(--_weaponPower, 0, 2);
    }

    private IEnumerator FireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            switch (_weaponPower)
            {
                case 0:
                    PoolManager.Release(_projectile1, _muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(_projectile1, _muzzleTop.position);
                    PoolManager.Release(_projectile1, _muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(_projectile2, _muzzleTop.position);
                    PoolManager.Release(_projectile1, _muzzleMiddle.position);
                    PoolManager.Release(_projectile3, _muzzleBottom.position);
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayRandomSfx(projectileSFXData);

            yield return new WaitForSeconds(_fireInterval);
        }
    }
    
    public void LaunchMissile()
    {
        _missleSystem.Launch(_muzzleMiddle);
    }

    public void PickUpMissile()
    {
        _missleSystem.PickUp();
    }
}