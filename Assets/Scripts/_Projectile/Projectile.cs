using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;
    protected GameObject target;
    
    [SerializeField] private float _damage;
    [SerializeField] private GameObject _hitVFX; 
    [SerializeField] private AudioData[] _hitAudioData;

    protected virtual void OnEnable() 
    {
        StartCoroutine(MoveDirectly());
    }

    private IEnumerator MoveDirectly() 
    {
        while (gameObject.activeSelf) 
        {
            Move();
            yield return null;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.TryGetComponent(out Character character)) 
        {
            character.TakeDamage(_damage);
            var contactPoint = other.GetContact(0); 
            
            PoolManager.Release(_hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            
            AudioManager.Instance.PlayRandomSfx(_hitAudioData);
            
            gameObject.SetActive(false);  
        }
    }

    protected void SetTarget(GameObject target) => this.target = target;
    public void Move() => transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
