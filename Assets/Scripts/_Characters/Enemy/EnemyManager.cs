using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => _enemyList.Count == 0 ? null : _enemyList[Random.Range(0, _enemyList.Count)];
    public int WaveNumber => _waveNumber;
    public float TimeBetweenWaves => _timeBetweenWaves;

    [Header("Wave")] 
    [SerializeField] private bool _spawnEnemy = true;
    [SerializeField] private GameObject _waveUI;
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private float _timeBetweenSpawns = 1f;
    [SerializeField] private float _timeBetweenWaves = 3f;
    [SerializeField] private int _minEnemyAmount = 2;
    [SerializeField] private int _maxEnemyAmount = 7;

    private int _waveNumber = 1;
    private int _enemyAmount;
    private List<GameObject> _enemyList;
    private WaitForSeconds _waitTimeBetweenSpawns;
    private WaitForSeconds _waitTimeBetweenWaves;
    private WaitUntil _waitUntilNoEnemy;

    protected override void Awake()
    {
        base.Awake();
        _enemyList = new List<GameObject>();
        _waitTimeBetweenSpawns = new WaitForSeconds(_timeBetweenSpawns);
        _waitTimeBetweenWaves = new WaitForSeconds(_timeBetweenWaves);
        _waitUntilNoEnemy = new WaitUntil(() => _enemyList.Count == 0);
    }

    private IEnumerator Start()
    {
        while (_spawnEnemy && GameManager.GameState != GameState.GameOver)
        {
            _waveUI.gameObject.SetActive(true);
            yield return _waitTimeBetweenWaves;
            _waveUI.gameObject.SetActive(false);
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
    }

    IEnumerator RandomlySpawnCoroutine()
    {
        _enemyAmount = Random.Range(_minEnemyAmount, _maxEnemyAmount);
        for (int i = 0; i < _enemyAmount; i++)
        {
            _enemyList.Add(PoolManager.Release(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]));
            yield return _waitTimeBetweenSpawns;
        }

        yield return _waitUntilNoEnemy;
        _waveNumber++;
    }

    public void RemoveFromList(GameObject enemy) => _enemyList.Remove(enemy);

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}