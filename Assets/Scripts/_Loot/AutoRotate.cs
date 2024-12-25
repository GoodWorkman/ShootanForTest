using System.Collections;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private float _speed = 360f;
    [SerializeField] private Vector3 _angle;

    private void OnEnable()
    {
        StartCoroutine(nameof(RotateCoroutine));
    }

    private IEnumerator RotateCoroutine()
    {
        while (gameObject.activeSelf)
        {
            transform.Rotate(_angle * _speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}