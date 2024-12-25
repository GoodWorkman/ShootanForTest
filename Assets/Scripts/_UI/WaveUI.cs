using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    private Text _waveText;

    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        _waveText = GetComponentInChildren<Text>();
    }
    
    void OnEnable()
    {
        _waveText.text = "-ROUND " + EnemyManager.Instance.WaveNumber + " -";
    }
}