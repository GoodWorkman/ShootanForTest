using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected float maxHealth;
    
    [SerializeField] private StatesBar _onHeadHealthBar;
    [SerializeField] private bool _showOnHeadHealthBar = true;
    [SerializeField] private AudioData[] _deathAudioData;
    [SerializeField] private GameObject _deathVFX; 

    protected float health;

    protected virtual void OnEnable() 
    {
        health = maxHealth;
        if (_showOnHeadHealthBar) ShowOnHeadHealthBar();
        else HideOnHeadHealthBar();
    }
    
    private void ShowOnHeadHealthBar() 
    {
        _onHeadHealthBar.gameObject.SetActive(true);
        _onHeadHealthBar.Initialize(health, maxHealth);
    }
    
    private void HideOnHeadHealthBar() 
    {
        _onHeadHealthBar.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage) 
    {
        if (health == 0) return;  
        
        health -= damage;
        
        if (_showOnHeadHealthBar) {
            _onHeadHealthBar.UpdateStates(health, maxHealth);
        }
        if (health <= 0f) {
            Die();
        }
    }

    public virtual void Die() 
    {
        AudioManager.Instance.PlayRandomSfx(_deathAudioData);
        health = 0f;
        PoolManager.Release(_deathVFX, transform.position);
        gameObject.SetActive(false);
    }

    public virtual void RestoreHealth(float value) 
    {
        if (health == maxHealth) return;
        health = Mathf.Clamp(health + value, 0, maxHealth);

        if (_showOnHeadHealthBar) {
            _onHeadHealthBar.UpdateStates(health, maxHealth);
        }
    }
   
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent) 
    {
        while (health < maxHealth) {
            yield return waitTime;
            RestoreHealth(maxHealth * percent);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
