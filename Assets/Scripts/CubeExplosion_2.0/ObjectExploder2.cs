using UnityEngine;

public class ObjectExploder2 : MonoBehaviour
{
    [SerializeField] private int _minimumInstantiatedObjectsNumber;
    [SerializeField] private int _maximumInstantiatedObjectsNumber;
    [SerializeField] private int _currentDivideChance;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private Color[] _colors;
    [SerializeField] private int _baseExplosionForce;
    [SerializeField] private float _baseExplosionRaduis;

    private MeshRenderer _meshRenderer;
    private int _minimumColorsNumber = 0;
    private int _minimumDivideChance = 0;
    private int _maximumDivideChance = 100;
    private int _randomDivideChanceNumber;
    private int _randomInstantiatedObjectsNumber;
    private int _randomColorNumber;
    private int _spaceDimensionsNumber = 3;
    private float _scaleAfterDivideMultiplier = 0.5f;
    private float _averageScaleValue;
    private float _localScaleExplosionForceCoefficient;
    private float _localScaleExplosionRadiusCoefficient;
    private float _distanceExplosionForceCoefficient;
    private bool _isObjectDivide = false;

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
            _isObjectDivide = true;
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

    private float GetDistanceBetweenObjects(Transform firstGameObjectTransform, Transform secondGameObjectTransform)
    {
        Vector3 vectorFirstGameObjectToSecondGameObject = secondGameObjectTransform.position - firstGameObjectTransform.position;
        float distance = vectorFirstGameObjectToSecondGameObject.magnitude;
        return distance;
    }

    public void Explode()
    {
        SpawnDividedGameObjects();
        
        if (_isObjectDivide == false)
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
            
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        }
    }
}