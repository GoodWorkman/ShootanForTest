using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Move")] [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _moveRotationAngle = 25f;

    [Header("Fire")] [SerializeField] protected GameObject[] projectiles;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected ParticleSystem muzzleVFX;
    [SerializeField] protected float minFireInterval;
    [SerializeField] protected float maxFireInterval;
    [SerializeField] protected AudioData[] fireAudioData;

    protected float paddingX;
    protected float paddingY;
    protected Vector3 targetPosition;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected virtual void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2;
        paddingY = size.y / 2;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyfireCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = ViewPort.Instance.RandomEnemySpawnPosition(paddingX, paddingY);
        targetPosition = ViewPort.Instance.RandomRightHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.position, targetPosition) > _moveSpeed * Time.fixedDeltaTime)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.fixedDeltaTime);
                transform.rotation = Quaternion.AngleAxis(
                    (targetPosition - transform.position).normalized.y * _moveRotationAngle,
                    Vector3.right
                );
            }
            else
            {
                targetPosition = ViewPort.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }

            yield return waitForFixedUpdate;
        }
    }

    private IEnumerator RandomlyfireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            if (GameManager.GameState == GameState.GameOver) yield break;

            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }

            AudioManager.Instance.PlayRandomSfx(fireAudioData);
            muzzleVFX.Play();
        }
    }
}