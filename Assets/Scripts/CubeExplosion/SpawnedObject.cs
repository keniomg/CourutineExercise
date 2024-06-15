using UnityEngine;

[RequireComponent (typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]

public class SpawnedObject : MonoBehaviour
{
    [SerializeField] private int _currentDivideChance;
    [SerializeField] private ObjectSpawner _spawner;

    private void DecreaseCurrentDivideChance()
    {
        int divideChanceDivider = 2;
        _currentDivideChance /= divideChanceDivider;
    }

    private void OnEnable()
    {
        _spawner.ObjectSpawnStarted += DecreaseCurrentDivideChance;
    }

    private void OnDisable()
    {
        _spawner.ObjectSpawnStarted -= DecreaseCurrentDivideChance;
    }

    public int GetCurrentDivideChance()
    {
        return _currentDivideChance;
    }
}