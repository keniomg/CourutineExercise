using UnityEngine;
using System;

public class ObjectExploder : MonoBehaviour
{
    [SerializeField] private float _explosionRaduis;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private int _explosionForce;
    [SerializeField] ObjectSpawner _spawner;

    public event Action ObjectExplode;

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
        ObjectExplode?.Invoke();
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