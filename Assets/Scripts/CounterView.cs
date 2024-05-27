using System.Collections;
using TMPro;
using UnityEngine;

public class CounterView : MonoBehaviour
{
    [SerializeField] private Counter _counter;
    [SerializeField] private TextMeshProUGUI _counterText;
    [SerializeField] private float _smoothValueIncreaseDuration = 0.5f;
    [SerializeField] private Animator _counterAnimator;
    [SerializeField] private AnimationClip _counterScalingAnimation;

    private void Start()
    {
        _counterText.text = _counter.CurrentValue.ToString();
    }

    private void OnEnable()
    {
        _counter.Changed += Count;
    }

    private void OnDisable()
    {
        _counter.Changed -= Count;
    }

    private void Count(float currentValue)
    {
        _counterAnimator.Play(_counterScalingAnimation.name);
        StartCoroutine(CountSmoothly(currentValue));
    }

    private IEnumerator CountSmoothly(float currentValue)
    {
        float previousValue = float.Parse(_counterText.text);
        float elapsedTime = 0f;

        while (elapsedTime < _smoothValueIncreaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / _smoothValueIncreaseDuration;
            float intermediatePosition = Mathf.Lerp(previousValue, currentValue, normalizedPosition);
            _counterText.text = intermediatePosition.ToString();
            
            yield return null;
        }
    }
}