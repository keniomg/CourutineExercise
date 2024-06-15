using System;
using UnityEngine;

public class ObjectExploder2 : ObjectExploder
{
    private bool _isObjectSpawned;

    private void Awake()
    {
        _isObjectSpawned = false;
    }

    private void OnEnable()
    {
        _spawner.ObjectSpawnEnded += SetObjectSpawnedStatusTrue;
    }

    private void OnDisable()
    {
        _spawner.ObjectSpawnEnded -= SetObjectSpawnedStatusTrue;
    }

    private void OnMouseDown()
    {
        Explode();
    }

    private float GetDistanceBetweenObjects(Transform firstGameObjectTransform, Transform secondGameObjectTransform)
    {
        Vector3 vectorFirstGameObjectToSecondGameObject = secondGameObjectTransform.position - firstGameObjectTransform.position;
        float distance = vectorFirstGameObjectToSecondGameObject.magnitude;
        return distance;
    }

    private void Explode()
    {
        InvokeObjectExploded();

        if (_isObjectSpawned == false)
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            AddExplosionForce();
        }

        Destroy(gameObject);
    }

    private void SetObjectSpawnedStatusTrue()
    {
        _isObjectSpawned = true;
    }

    private void AddExplosionForce()
    {
        int spaceDimensionsNumber = 3;
        float averageScaleValue = (transform.localScale.x + transform.localScale.x + transform.localScale.x) / spaceDimensionsNumber;
        float localScaleExplosionForceCoefficient = 1 / averageScaleValue;
        float localScaleExplosionRadiusCoefficient = 1 / averageScaleValue;
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRaduis);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out ObjectExploder2 explodableObject))
            {
                float distanceExplosionForceCoefficient = 1 / GetDistanceBetweenObjects(gameObject.transform, explodableObject.transform);
                float totalExplosionForce = _explosionForce * localScaleExplosionForceCoefficient;
                float totalExplosionRaduis = distanceExplosionForceCoefficient * localScaleExplosionRadiusCoefficient;
                explodableObject.GetComponent<Rigidbody>().AddExplosionForce(totalExplosionForce, transform.position, totalExplosionRaduis);
            }
        }
    }
}