using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _explosionRaduis;
    [SerializeField] private int _minimumInstantiatedObjectsNumber;
    [SerializeField] private int _maximumInstantiatedObjectsNumber;
    [SerializeField] private int _currentDivideChance;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private Color[] _colors;
    [SerializeField] private int _explosionForce;

    private int _minimumColorsNumber = 0;
    private int _minimumDivideChance = 0;
    private int _maximumDivideChance = 100;
    private int _randomDivideChanceNumber;
    private int _randomInstantiatedObjectsNumber;
    private int _randomColorNumber;
    private float _scaleAfterDivideMultiplier = 0.5f;

    private void OnMouseDown()
    {
        Explode();
    }

    private void SpawnDividedGameObjects()
    {
        _randomDivideChanceNumber = Random.Range(_minimumDivideChance, _maximumDivideChance);
        _randomInstantiatedObjectsNumber = Random.Range(_minimumInstantiatedObjectsNumber, _maximumInstantiatedObjectsNumber);

        if (_randomDivideChanceNumber <= _currentDivideChance)
        {
            transform.localScale *= _scaleAfterDivideMultiplier;
            _currentDivideChance /= 2;
            ChangeObjectColor(gameObject);

            for (int i = 0; i < _randomInstantiatedObjectsNumber; i++)            
            {
                GameObject objectInstantiatedByExplosion = Instantiate(this.gameObject, transform.position, Quaternion.identity);
                ChangeObjectColor(objectInstantiatedByExplosion);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ChangeObjectColor(GameObject gameObject)
    {
        _randomColorNumber = Random.Range(_minimumColorsNumber, _colors.Length);
        gameObject.GetComponent<MeshRenderer>().material.color = _colors[_randomColorNumber];
    }

    public void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRaduis);
        SpawnDividedGameObjects();

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Exploder explodableObject))
            {
                explodableObject.GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, transform.position, _explosionRaduis);
            }
        }

        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}