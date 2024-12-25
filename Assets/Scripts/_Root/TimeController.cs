using System.Collections;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0, 1)] 
    private float _bulletTimeScale = 0.1f;

    private float _defualtFixedDeltaTime;
    private float _timeScaleBeforePause;
    private float _time;
    private float _secondByStep = 1f;

    protected override void Awake()
    {
        base.Awake();
        _defualtFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void BulletTime(float duration)
    {
        Time.timeScale = _bulletTimeScale;
        StartCoroutine(SlowOffCoroutine(duration));
    }

    public void BulletTime(float onDuration, float offDuration)
    {
        StartCoroutine(SlowOnAndOffCoroutine(onDuration, offDuration));
    }

    private IEnumerator SlowOnAndOffCoroutine(float onDuration, float offDuration)
    {
        yield return StartCoroutine(SlowOnCoroutine(onDuration));
        StartCoroutine(SlowOffCoroutine(offDuration));
    }

    private IEnumerator SlowOnCoroutine(float duration)
    {
        _time = 0f;
        while (_time < _secondByStep)
        {
            _time += Time.unscaledDeltaTime / duration;

            Time.timeScale = Mathf.Lerp(_secondByStep, _bulletTimeScale, _time);

            Time.fixedDeltaTime = _defualtFixedDeltaTime * Time.timeScale;

            yield return null;
        }
    }

    private IEnumerator SlowOffCoroutine(float duration)
    {
        _time = 0f;
        while (_time < _secondByStep)
        {
            _time += Time.unscaledDeltaTime / duration;
            
            Time.timeScale = Mathf.Lerp(_bulletTimeScale, _secondByStep, _time);
            
            Time.fixedDeltaTime = _defualtFixedDeltaTime * Time.timeScale;

            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}