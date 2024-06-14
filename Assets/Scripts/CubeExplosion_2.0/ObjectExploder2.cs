using System;
using UnityEngine;

public class ObjectExploder2 : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private int _baseExplosionForce;
    [SerializeField] private float _baseExplosionRaduis;
    [SerializeField] private ObjectSpawner _spawner;

    private int _spaceDimensionsNumber = 3;
    private float _averageScaleValue;
    private float _localScaleExplosionForceCoefficient;
    private float _localScaleExplosionRadiusCoefficient;
    private float _distanceExplosionForceCoefficient;
    private bool _isObjectSpawned;

    public Action ObjectExplode;

    private void Awake()
    {
        _isObjectSpawned = false;
    }

    private void OnEnable()
    {
        _spawner.ObjectSpawned += SetObjectSpawnedStatusTrue;
    }

    private void OnDisable()
    {
        _spawner.ObjectSpawned -= SetObjectSpawnedStatusTrue;
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
        _averageScaleValue = (transform.localScale.x + transform.localScale.x + transform.localScale.x) / _spaceDimensionsNumber;
        _localScaleExplosionForceCoefficient = 1 / _averageScaleValue;
        _localScaleExplosionRadiusCoefficient = 1 / _averageScaleValue;
        float totalExplosionForce = _baseExplosionForce * _localScaleExplosionForceCoefficient;
        float totalExplosionRaduis = _distanceExplosionForceCoefficient * _localScaleExplosionRadiusCoefficient;
        Collider[] hits = Physics.OverlapSphere(transform.position, _baseExplosionRaduis);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out ObjectExploder2 explodableObject))
            {
                _distanceExplosionForceCoefficient = 1 / GetDistanceBetweenObjects(gameObject.transform, explodableObject.transform);
                explodableObject.GetComponent<Rigidbody>().AddExplosionForce(totalExplosionForce, transform.position, totalExplosionRaduis);
            }
        }
    }
}