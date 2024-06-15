using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class ObjectColorChanger : MonoBehaviour
{
    [SerializeField] private Color[] _colors;
    [SerializeField] private ObjectSpawner _spawner;

    private void OnEnable()
    {
        _spawner.ObjectSpawnEnded += ChangeObjectColor;
    }

    private void OnDisable()
    {
        _spawner.ObjectSpawnEnded -= ChangeObjectColor;
    }

    private void ChangeObjectColor()
    {
        List<MeshRenderer> spawnedObjectsRenderer = new List<MeshRenderer>();
        int minimumColorsNumber = 0;
        IReadOnlyList<SpawnedObject> spawnedObjects = _spawner.GetSpawnedObjects();

        foreach (SpawnedObject divideableObject in spawnedObjects)
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