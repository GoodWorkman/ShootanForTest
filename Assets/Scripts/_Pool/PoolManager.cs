using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Pool[] vFXPools;
    [SerializeField] private Pool[] lootItemPools;
    [SerializeField] private Pool[] enemyPools;
    [SerializeField] private Pool[] playerProjectilePools;
    [SerializeField] private Pool[] enemyProjectilePools;
    static Dictionary<GameObject, Pool> prefab2Pool;  

    private void Awake() {
        prefab2Pool = new Dictionary<GameObject, Pool>();
        Initialize(enemyPools);
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(vFXPools);
        Initialize(lootItemPools);
    }

    #if UNITY_EDITOR
        private void OnDestroy() {
            CheckPoolSize(enemyPools);
            CheckPoolSize(playerProjectilePools);
            CheckPoolSize(enemyProjectilePools);
            CheckPoolSize(vFXPools);
            CheckPoolSize(lootItemPools);
        }
    #endif

    void CheckPoolSize(Pool[] pools) {
        foreach (var pool in pools) {
            if (pool.RuntimeSize > pool.Size) {
                Debug.LogWarning(
                    $"Pool: {pool.Prefab.name} has a runtime size {pool.RuntimeSize} bigger " +
                    $"than its initial size {pool.Size}!"
                );
            }
        }
    }

    void Initialize(Pool[] pools) {
        foreach (var pool in pools) {
            
            #if UNITY_EDITOR
                if (prefab2Pool.ContainsKey(pool.Prefab)) {
                    Debug.LogError("Duplicate in Pools! Prefab: " + pool.Prefab.name);
                    continue;  
                }
            #endif
            prefab2Pool.Add(pool.Prefab, pool);
            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;
            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }
    public static GameObject Release(GameObject prefab) {
        #if UNITY_EDITOR
            if (!prefab2Pool.ContainsKey(prefab)) {
                Debug.LogError("Missing Pool prefab prefab: " + prefab.name);
                return null;
            }
        #endif
        return prefab2Pool[prefab].PreparedObject();
    }

    public static void Release(GameObject prefab, Vector3 position) {
        #if UNITY_EDITOR
            if (!prefab2Pool.ContainsKey(prefab))
            {
                Debug.LogError("Missing Pool prefab prefab: " + prefab.name);
                return;
            }
        #endif
        prefab2Pool[prefab].PreparedObject(position);
    }

    public static void Release(GameObject prefab, Vector3 position, Quaternion rotation) {
        #if UNITY_EDITOR
            if (!prefab2Pool.ContainsKey(prefab))
            {
                Debug.LogError("Missing Pool prefab prefab: " + prefab.name);
                return;
            }
        #endif
        prefab2Pool[prefab].PreparedObject(position, rotation);
    }
}
