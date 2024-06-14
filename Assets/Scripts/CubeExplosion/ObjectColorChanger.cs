using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class ObjectColorChanger : MonoBehaviour
{
    [SerializeField] private Color[] _colors;
    [SerializeField] private ObjectSpawner _spawner;

    private List<SpawnedObject> _spawnedObjects;

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
        List<MeshRenderer> spawnedObjectsRenderer = new List<MeshRenderer>();
        int minimumColorsNumber = 0;
        _spawnedObjects = _spawner.GetSpawnedObjects();

        foreach (SpawnedObject divideableObject in _spawnedObjects)
        {
            spawnedObjectsRenderer.Add(divideableObject.GetComponent<MeshRenderer>());
        }

        if (spawnedObjectsRenderer != null)
        {
            foreach (MeshRenderer divideableObjectRenderer in spawnedObjectsRenderer)
            {
                int randomColorNumber = Random.Range(minimumColorsNumber, _colors.Length);
                divideableObjectRenderer.material.color = _colors[randomColorNumber];
            }
        }
    }
}