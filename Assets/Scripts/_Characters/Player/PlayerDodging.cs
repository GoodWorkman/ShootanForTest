using System;
using System.Collections;
using UnityEngine;

public class PlayerDodging : MonoBehaviour
{
    [SerializeField] private AudioData _dodgeAudioData;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private int _dodgeEnergyCost = 25;
    [SerializeField] private float _maxRoll = 720f;
    [SerializeField] private float _rollSpeed = 360f;
    [SerializeField] private Vector3 _dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private float _slowMotionDuration = 1f;

    private bool _isDodging;
    private float currentRoll;
    private float dodgeDuration;

    private void Awake()
    {
        dodgeDuration = _maxRoll / _rollSpeed;
    }

    public void Dodge()
    {
        if (_isDodging|| !PlayerEnergy.Instance.IsEnough(_dodgeEnergyCost)) return;

        StartCoroutine(DodgeCoroutine());
    }

    private IEnumerator DodgeCoroutine()
    {
        _isDodging = true;
        AudioManager.Instance.PlaySfx(_dodgeAudioData);

        PlayerEnergy.Instance.Use(_dodgeEnergyCost);

        _collider2D.isTrigger = true;

        currentRoll = 0f;
        var scale = transform.localScale;

        TimeController.Instance.BulletTime(_slowMotionDuration, _slowMotionDuration);

        var t1 = 0f;
        var t2 = 0f;

        while (currentRoll < _maxRoll)
        {
            currentRoll += _rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

            if (currentRoll < _maxRoll / 2)
            {
                t1 += Time.deltaTime / dodgeDuration;
                transform.localScale = Vector3.Lerp(transform.localScale, _dodgeScale, t1);
            }
            else
            {
                t2 += Time.deltaTime / dodgeDuration;
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t2);
            }

            yield return null;
        }

        _collider2D.isTrigger = false;
        _isDodging = false;
    }
}