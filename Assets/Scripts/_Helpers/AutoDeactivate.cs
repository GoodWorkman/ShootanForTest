using System.Collections;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] private bool _destroyGameObject;
    [SerializeField] private float _lifetime = 3f;

    private WaitForSeconds _waitLifetime;

    private void Awake()
    {
        _waitLifetime = new WaitForSeconds(_lifetime);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    private IEnumerator DeactivateCoroutine()
    {
        yield return _waitLifetime;

        if (_destroyGameObject)
        {
            Destroy(gameObject);
        }
        else 
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}