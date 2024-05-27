using System;
using System.Collections;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private float _startValue;
    [SerializeField] private float _valueStep;
    [SerializeField] private float _timeStep;

    private float _currentValue;
    private bool _isCount = false;
    private Coroutine _count;

    public float CurrentValue => _currentValue;

    public event Action<float> Changed;

    private void Start()
    {
        _currentValue = _startValue;
    }

    private IEnumerator Count()
    {
        while (_isCount)
        {
            _currentValue += _valueStep;
            Changed?.Invoke(_currentValue);
            Debug.Log(CurrentValue);

            yield return new WaitForSeconds(_timeStep);
        }
    }

    public void ChangeCountStatus()
    {
        _isCount = !_isCount;

        if (_isCount)
        {
            _count = StartCoroutine(Count());
        }
        else
        {
            if (_count != null)
            {
                StopCoroutine(_count);
                _count = null;
            }
        }
    }
}