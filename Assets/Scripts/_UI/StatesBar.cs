using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatesBar : MonoBehaviour
{
    [SerializeField] private Image _fillImageBack;
    [SerializeField] private Image _fillImageFront;
    [SerializeField] private float _fillSpeed = 0.1f;
    [SerializeField] private bool _delayFill = true;
    [SerializeField] private float _fillDelay = 0.5f;

    protected float currentFillAmount;
    protected float targetFillAmount;
    
    private float _time;
    private WaitForSeconds _waitForDelayFill;
    private Coroutine _bufferedfillingCoroutine;


    private void Awake()
    {
        _waitForDelayFill = new WaitForSeconds(_fillDelay);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        _fillImageBack.fillAmount = currentFillAmount;
        _fillImageFront.fillAmount = currentFillAmount;
    }

    public virtual void UpdateStates(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue;

        if (_bufferedfillingCoroutine != null)
        {
            StopCoroutine(_bufferedfillingCoroutine);
        }

        if (currentFillAmount > targetFillAmount)
        {
            _fillImageFront.fillAmount = targetFillAmount;
            _bufferedfillingCoroutine = StartCoroutine(
                BufferedFillingCoroutine(_fillImageBack));

            return;
        }

        if (currentFillAmount < targetFillAmount)
        {
            _fillImageBack.fillAmount = targetFillAmount;
            _bufferedfillingCoroutine = StartCoroutine(
                BufferedFillingCoroutine(_fillImageFront)
            );
        }
    }

    private IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (_delayFill)
        {
            yield return _waitForDelayFill;
        }

        _time = 0f;
        while (_time < 1f)
        {
            _time += Time.deltaTime * _fillSpeed;

            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, _time);
            image.fillAmount = currentFillAmount;

            yield return null;
        }
    }
}