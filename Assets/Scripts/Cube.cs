using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(BoxCollider), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private float _startSeparationChance = 1f;
    [SerializeField] private float _startExplosuionRadius = 5f;
    [SerializeField] private float _startExplosuionForce = 100f;
    [SerializeField] private float _startGlobalExplosuionRadius = 5f;
    [SerializeField] private float _startGlobalExplosuionForce = 100f;

    private Renderer _renderer;
    private BoxCollider _collider;
    private Rigidbody _rb;
    private float _separationChance;
    private float _explosuionRadius;
    private float _explosuionForce;
    private float _globalExplosuionRadius;
    private float _globalExplosuionForce;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();

        _separationChance = _startSeparationChance;
        _explosuionRadius = _startExplosuionRadius;
        _explosuionForce = _startExplosuionForce;
        _globalExplosuionRadius = _startGlobalExplosuionRadius;
        _globalExplosuionForce = _startGlobalExplosuionForce;
    }

    private void OnMouseUpAsButton()
    {
        if (_separationChance > Random.Range(0, 1f))
        {
            IEnumerable<Rigidbody> explodableCubes = SpawnCubes()
            .Select(cube => cube.GetComponent<Rigidbody>());

            foreach (Rigidbody explodableCube in explodableCubes)
                explodableCube.AddExplosionForce(_explosuionForce, transform.position,
                    _explosuionRadius);
        }
        else
        {
            List<Rigidbody> explodableObjects = GetExplodableObjects();
            foreach (Rigidbody explodableObject in explodableObjects)
                explodableObject.AddExplosionForce(_globalExplosuionForce, 
                    transform.position, _globalExplosuionRadius);
        }

        Destroy(gameObject);
    }

    public void Init(Vector3 prevScale, float prevMass, float prevSeparationChance,
        float prevExplosuionForce, float prevExplosuionRadius,
        float prevGlobalExplosuionForce, float prevGlobalExplosuionRadius)
    {
        transform.localScale = prevScale / 2;

        _rb.mass = prevMass / 2;

        _renderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        _separationChance = prevSeparationChance / 2;
        _explosuionForce = prevExplosuionForce / 2;
        _explosuionRadius = prevExplosuionRadius / 2;
        _globalExplosuionForce = prevGlobalExplosuionForce * 2;
        _globalExplosuionRadius = prevGlobalExplosuionRadius * 2;
    }

    private Cube[] SpawnCubes()
    {
        int newCubesCount = Random.Range(2, 7);

        Cube[] cubes = new Cube[newCubesCount];

        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i] = Instantiate(this, transform.parent);

            cubes[i].Init(transform.localScale, _rb.mass, _separationChance, 
                _explosuionForce, _explosuionRadius, _globalExplosuionForce, 
                _globalExplosuionRadius);
        }

        return cubes;
    }

    private List<Rigidbody> GetExplodableObjects()
    {
        var explodableObjects = new List<Rigidbody>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, _globalExplosuionRadius);

        foreach (Collider collider in colliders)
            if(collider.TryGetComponent(out Rigidbody rigidbody))
                explodableObjects.Add(rigidbody);

        return explodableObjects;
    }
}
