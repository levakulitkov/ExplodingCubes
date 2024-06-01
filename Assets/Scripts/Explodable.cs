using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Explodable : MonoBehaviour
{
    [SerializeField] private float _startSeparationRadius = 5f;
    [SerializeField] private float _startSeparationForce = 100f;
    [SerializeField] private float _startGlobalExplosuionRadius = 5f;
    [SerializeField] private float _startGlobalExplosuionForce = 100f;

    public Rigidbody RigidBody { get; private set; }
    public float SeparationRadius { get; private set; }
    public float SeparationForce { get; private set; }
    public float GlobalExplosionRadius { get; private set; }
    public float GlobalExplosionForce { get; private set; }

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();

        SeparationRadius = _startSeparationRadius;
        SeparationForce = _startSeparationForce;
        GlobalExplosionRadius = _startGlobalExplosuionRadius;
        GlobalExplosionForce = _startGlobalExplosuionForce;
    }

    public void Init(Explodable prevExplodable)
    {
        RigidBody.mass = prevExplodable.RigidBody.mass / 2;
        SeparationForce = prevExplodable.SeparationForce / 2;
        SeparationRadius = prevExplodable.SeparationRadius / 2;
        GlobalExplosionForce = prevExplodable.GlobalExplosionForce * 2;
        GlobalExplosionRadius = prevExplodable.GlobalExplosionRadius * 2;
    }

    public void Explode()
    {
        List<Rigidbody> explodableObjects = GetExplodableObjects();
        foreach (Rigidbody explodableObject in explodableObjects)
            explodableObject.AddExplosionForce(GlobalExplosionForce,
                transform.position, GlobalExplosionRadius);
    }

    public void Separate(Cube[] cubes)
    {
        IEnumerable<Rigidbody> separatableCubes = cubes
                .Select(cube => cube.Explodable.RigidBody);

        foreach (Rigidbody separatable in separatableCubes)
            separatable.AddExplosionForce(SeparationForce,
                transform.position, SeparationRadius);
    }

    private List<Rigidbody> GetExplodableObjects()
    {
        var explodableObjects = new List<Rigidbody>();

        Collider[] colliders = Physics.OverlapSphere(transform.position,
            GlobalExplosionRadius);

        foreach (Collider collider in colliders)
            if (collider.TryGetComponent(out Rigidbody rigidbody))
                explodableObjects.Add(rigidbody);

        return explodableObjects;
    }
}