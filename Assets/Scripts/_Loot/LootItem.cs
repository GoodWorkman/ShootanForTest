using System.Collections;
using UnityEngine;

public class LootItem : MonoBehaviour
{
    [SerializeField] protected AudioData defaultPickUpSFX;
    
    [SerializeField] private float _minSpeed = 5f;
    [SerializeField] private float _maxSpeed = 15f;

    protected PlayerCore playerCore;
    protected PlayerShooting playerShooting;
    protected AudioData pickUpSFX;

    private void Awake()
    {
        playerCore = FindObjectOfType<PlayerCore>();
        playerShooting = FindObjectOfType<PlayerShooting>();
        gameObject.SetActive(true);

        pickUpSFX = defaultPickUpSFX;
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(MoveCoroutine));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PickUp();
    }

    protected virtual void PickUp()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        AudioManager.Instance.PlayRandomSfx(pickUpSFX);
    }

    private IEnumerator MoveCoroutine()
    {
        float speed = Random.Range(_minSpeed, _maxSpeed);
        Vector3 direction = Vector3.left;

        while (gameObject.activeSelf)
        {
            if (playerCore.isActiveAndEnabled || playerShooting.isActiveAndEnabled)
            {
                direction = (playerShooting.transform.position - transform.position).normalized;
            }

            transform.Translate(direction * speed * Time.deltaTime);
            yield return null;
        }
    }
}