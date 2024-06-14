using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent (typeof(SpawnedObject))]

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private int _minimumInstantiatedObjectsNumber;
    [SerializeField] private int _maximumInstantiatedObjectsNumber;
    [SerializeField] private SpawnedObject _spawnedObject;
    [SerializeField] private ObjectExploder _exploder;

    private List<SpawnedObject> _spawnedObjects;
    public event Action ObjectSpawn;
    public event Action ObjectSpawned;

    private void OnEnable()
    {
        _exploder.ObjectExploded += SpawnDividedGameObjects;
    }

    private void OnDisable()
    {
        _exploder.ObjectExploded -= SpawnDividedGameObjects;
    }

    private void SpawnDividedGameObjects()
    {
        _spawnedObjects = new List<SpawnedObject>();
        float scaleAfterDivideMultiplier = 0.5f;
        int minimumDivideChance = 0;
        int maximumDivideChance = 100;
        int randomDivideChanceNumber = Random.Range(minimumDivideChance, maximumDivideChance);

        if (randomDivideChanceNumber <= _spawnedObject.GetCurrentDivideChance())
        {
            int randomInstantiatedObjectsNumber = Random.Range(_minimumInstantiatedObjectsNumber, _maximumInstantiatedObjectsNumber);
            _spawnedObject.transform.localScale *= scaleAfterDivideMultiplier;
            ObjectSpawn?.Invoke();

            for (int i = 0; i < randomInstantiatedObjectsNumber; i++)
            {
                _spawnedObjects.Add(Instantiate(_spawnedObject, transform.position, Quaternion.identity));
            }

            ObjectSpawned?.Invoke();
        }
        else
        {
            _spawnedObjects = null;
        }
    }

    public List<SpawnedObject> GetSpawnedObjects()
    {
        List<SpawnedObject> spawnedObjects = new List<SpawnedObject>();
        return spawnedObjects = _spawnedObjects;
    }
}