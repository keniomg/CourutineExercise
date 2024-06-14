using UnityEngine;
using System;

public class ObjectExploder : MonoBehaviour
{
    [SerializeField] private float _explosionRaduis;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private int _explosionForce;
    [SerializeField] private ObjectSpawner _spawner;

    public event Action ObjectExploded;

    private void OnMouseDown()
    {
        Explode();
    }

    private void OnEnable()
    {
        _spawner.ObjectSpawned += AddExplosionForce;
    }

    private void OnDisable()
    {
        _spawner.ObjectSpawned -= AddExplosionForce;
    }

    private void Explode()
    {
        ObjectExploded?.Invoke();
        Destroy(gameObject);
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
    }

    private void AddExplosionForce()
    {
        foreach (SpawnedObject spawnedObject in _spawner.GetSpawnedObjects())
        {
            spawnedObject.GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, transform.position, _explosionRaduis);
        }
    }
}