using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private LootSetting[] _lootSettings;

    public void Spawn(Vector2 position)
    {
        foreach (var item in _lootSettings)
        {
            item.Spawn(position + Random.insideUnitCircle);
        }
    }
}