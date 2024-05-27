using System;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private float _startValue;
    [SerializeField] private float _stepValue;

    private float _currentValue;

    public float CurrentValue => _currentValue;

    public event Action<float> Changed;

    private void Start()
    {
        _currentValue = _startValue;
    }
    
    public void Count()
    {
        _currentValue += _stepValue;
        Changed?.Invoke(_currentValue);
        Debug.Log(CurrentValue);
    }
}