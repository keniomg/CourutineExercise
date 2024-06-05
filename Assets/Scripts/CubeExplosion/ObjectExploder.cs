using UnityEngine;

public class ObjectExploder : MonoBehaviour
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
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void OnMouseDown()
    {
        Explode();
    }

    private void SpawnDividedGameObjects()
    {
        _randomDivideChanceNumber = Random.Range(_minimumDivideChance, _maximumDivideChance);

        if (_randomDivideChanceNumber <= _currentDivideChance)
        {
            _randomInstantiatedObjectsNumber = Random.Range(_minimumInstantiatedObjectsNumber, _maximumInstantiatedObjectsNumber);
            transform.localScale *= _scaleAfterDivideMultiplier;
            _currentDivideChance /= 2;

            for (int i = 0; i < _randomInstantiatedObjectsNumber; i++)            
            {
                MeshRenderer renderer = Instantiate(_meshRenderer, transform.position, Quaternion.identity);
                ChangeObjectColor(renderer);
            }
        }

        Destroy(gameObject);
    }

    private void ChangeObjectColor(MeshRenderer meshRenderer)
    {
        _randomColorNumber = Random.Range(_minimumColorsNumber, _colors.Length);
        meshRenderer.material.color = _colors[_randomColorNumber];
    }

    public void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRaduis);
        SpawnDividedGameObjects();

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out ObjectExploder explodableObject))
            {
                explodableObject.GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, transform.position, _explosionRaduis);
            }
        }

        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
    }
}