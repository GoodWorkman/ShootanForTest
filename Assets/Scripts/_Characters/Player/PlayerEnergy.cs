using System.Collections;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] private EnergyBar _energyBar;
    public const int MAX = 100;
    public const int PERCENT = 1;
    private int _energy;
    
    private bool _available = true;
    
    private void Start() 
    {
        Obtain(MAX);
        _energyBar.Initialize(_energy, MAX);
    }

    public bool IsEnough(int value) => _energy >= value;

    public void Obtain(int value) 
    {
        if (_energy >= MAX || !_available || !gameObject.activeSelf) return;
        _energy = Mathf.Clamp(_energy + value, 0, MAX);
        _energyBar.UpdateStates(_energy, MAX);
    } 

    public void Use(int value) 
    {
        _energy -= value;
        _energyBar.UpdateStates(_energy, MAX);
    }
}
