using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class ObjectColorChanger : MonoBehaviour
{
    [SerializeField] private Color[] _colors;
    [SerializeField] private ObjectSpawner _spawner;

    private List<SpawnedObject> _spawnedObjects;
    private List<MeshRenderer> _spawnedObjectsRenderer;
    private int _minimumColorsNumber = 0;
    private int _randomColorNumber;

    private void OnEnable()
    {
        _spawner.ObjectSpawned += ChangeObjectColor;
    }

    private void OnDisable()
    {
        _spawner.ObjectSpawned -= ChangeObjectColor;
    }

    private void ChangeObjectColor()
    {
        _spawnedObjects = _spawner.GetSpawnedObjects();
        _spawnedObjectsRenderer = new List<MeshRenderer>();

        foreach (SpawnedObject divideableObject in _spawnedObjects)
        {
            _spawnedObjectsRenderer.Add(divideableObject.GetComponent<MeshRenderer>());
        }

        if (_spawnedObjectsRenderer != null)
        {
            foreach (MeshRenderer divideableObjectRenderer in _spawnedObjectsRenderer)
            {
                _randomColorNumber = Random.Range(_minimumColorsNumber, _colors.Length);
                divideableObjectRenderer.material.color = _colors[_randomColorNumber];
            }
        }
    }
}