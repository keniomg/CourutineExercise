using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SizeDependingObjectExploder : ObjectExploder
{
    override protected void OnEnable()
    {
        Spawner.ObjectNotSpawned += InstantiateExplosion;
    }

    override protected void OnDisable()
    {
        Spawner.ObjectNotSpawned -= InstantiateExplosion;
    }

    override protected void Explode()
    {
        InvokeObjectExploded();
        Destroy(gameObject);
    }

    override protected void AddExplosionForce()
    {
        int spaceDimensionsNumber = 3;
        float averageScaleValue = (transform.localScale.x + transform.localScale.x + transform.localScale.x) / spaceDimensionsNumber;
        float localScaleExplosionForceCoefficient = 1 / averageScaleValue;
        float localScaleExplosionRadiusCoefficient = 1 / averageScaleValue;
        Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRaduis);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out SizeDependingObjectExploder explodableObject))
            {
                float distanceExplosionForceCoefficient = 1 / GetDistanceBetweenObjects(gameObject.transform, explodableObject.transform);
                float totalExplosionForce = ExplosionForce * localScaleExplosionForceCoefficient;
                float totalExplosionRaduis = distanceExplosionForceCoefficient * localScaleExplosionRadiusCoefficient;
                explodableObject.GetComponent<Rigidbody>().AddExplosionForce(totalExplosionForce, transform.position, totalExplosionRaduis);
            }
        }
    }

    private void InstantiateExplosion()
    {
        Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        AddExplosionForce();
    }

    private float GetDistanceBetweenObjects(Transform firstGameObjectTransform, Transform secondGameObjectTransform)
    {
        Vector3 vectorFirstGameObjectToSecondGameObject = secondGameObjectTransform.position - firstGameObjectTransform.position;
        float distance = vectorFirstGameObjectToSecondGameObject.magnitude;
        return distance;
    }
}