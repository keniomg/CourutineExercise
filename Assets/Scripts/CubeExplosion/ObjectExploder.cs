using UnityEngine;
using System;

public class ObjectExploder : MonoBehaviour
{
    [SerializeField] protected float _explosionRaduis;
    [SerializeField] protected ParticleSystem _explosionEffect;
    [SerializeField] protected int _explosionForce;
    [SerializeField] protected ObjectSpawner _spawner;

    public event Action ObjectExploded;

    private void OnMouseDown()
    {
        Explode();
    }

    private void OnEnable()
    {
        _spawner.ObjectSpawnEnded += AddExplosionForce;
    }

    private void OnDisable()
    {
        _spawner.ObjectSpawnEnded -= AddExplosionForce;
    }

    private void Explode()
    {
        InvokeObjectExploded();
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

    protected void InvokeObjectExploded()
    {
        ObjectExploded?.Invoke();
    }
}