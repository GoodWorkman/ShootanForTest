using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private float _minBallisticAngle = -90f;
    [SerializeField] private float _maxBallisticAngle = 90f;
    private Vector3 _targetDirection;
    private float _ballisticAngle;

    public IEnumerator HomingCoroutine(GameObject target) 
    {
        _ballisticAngle = Random.Range(_minBallisticAngle, _maxBallisticAngle);
        while (gameObject.activeSelf) {
            
            if (target.activeSelf) {
                
                _targetDirection = target.transform.position - transform.position;
                
                transform.rotation = Quaternion.AngleAxis(
                    Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg, 
                    Vector3.forward
                );
                transform.rotation *= Quaternion.Euler(0f, 0f, _ballisticAngle);
            }
            _projectile.Move();  

            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
