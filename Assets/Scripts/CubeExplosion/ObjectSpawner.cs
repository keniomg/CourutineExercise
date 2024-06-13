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
    private int _minimumDivideChance = 0;
    private int _maximumDivideChance = 100;
    private int _randomDivideChanceNumber;
    private int _randomInstantiatedObjectsNumber;
    private float _scaleAfterDivideMultiplier = 0.5f;
    public event Action ObjectSpawn;
    public event Action ObjectSpawned;

    private void OnEnable()
    {
        _exploder.ObjectExplode += SpawnDividedGameObjects;
    }

    private void OnDisable()
    {
        _exploder.ObjectExplode -= SpawnDividedGameObjects;
    }

    private void SpawnDividedGameObjects()
    {
        _spawnedObjects = new List<SpawnedObject>();
        _randomDivideChanceNumber = Random.Range(_minimumDivideChance, _maximumDivideChance);

        if (_randomDivideChanceNumber <= _spawnedObject.GetCurrentDivideChance())
        {
            _randomInstantiatedObjectsNumber = Random.Range(_minimumInstantiatedObjectsNumber, _maximumInstantiatedObjectsNumber);
            _spawnedObject.transform.localScale *= _scaleAfterDivideMultiplier;
            ObjectSpawn?.Invoke();

            for (int i = 0; i < _randomInstantiatedObjectsNumber; i++)
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