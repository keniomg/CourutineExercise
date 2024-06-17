using UnityEngine;
using System;

public class ObjectExploder : MonoBehaviour
{
    [SerializeField] protected float ExplosionRaduis;
    [SerializeField] protected ParticleSystem ExplosionEffect;
    [SerializeField] protected int ExplosionForce;
    [SerializeField] protected ObjectSpawner Spawner;

    public event Action ObjectExploded;

    protected void InvokeObjectExploded()
    {
        ObjectExploded?.Invoke();
    }

    virtual protected void OnMouseDown()
    {
        Explode();
    }

    virtual protected void OnEnable()
    {
        Spawner.ObjectSpawnEnded += AddExplosionForce;
    }

    virtual protected void OnDisable()
    {
        Spawner.ObjectSpawnEnded -= AddExplosionForce;
    }

    virtual protected void Explode()
    {
        InvokeObjectExploded();
        Destroy(gameObject);
        Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
    }

    virtual protected void AddExplosionForce()
    {
        foreach (SpawnedObject spawnedObject in Spawner.GetSpawnedObjects())
        {
            spawnedObject.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, ExplosionRaduis);
        }
    }
}