using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent (typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]

public class SpawnedObject : MonoBehaviour
{
    [SerializeField] private int _currentDivideChance;
    [SerializeField] private ObjectSpawner _spawner;

    private void DecreaseCurrentDivideChance()
    {
        _currentDivideChance /= 2;
    }

    private void OnEnable()
    {
        _spawner.ObjectSpawn += DecreaseCurrentDivideChance;
    }

    private void OnDisable()
    {
        _spawner.ObjectSpawn -= DecreaseCurrentDivideChance;
    }

    public int GetCurrentDivideChance()
    {
        int currentDivideChance = _currentDivideChance;
        return currentDivideChance;
    }
}