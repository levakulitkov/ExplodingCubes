using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Explodable : MonoBehaviour
{
    [SerializeField] private float _startSeparationRadius = 5f;
    [SerializeField] private float _startSeparationForce = 100f;

    public Rigidbody RigidBody { get; private set; }
    public float SeparationRadius { get; private set; }
    public float SeparationForce { get; private set; }

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();

        SeparationRadius = _startSeparationRadius;
        SeparationForce = _startSeparationForce;
    }

    public void Init(Explodable prevExplodable)
    {
        RigidBody.mass = prevExplodable.RigidBody.mass / 2;
        SeparationForce = prevExplodable.SeparationForce / 2;
        SeparationRadius = prevExplodable.SeparationRadius / 2;
    }

    public void Separate(Cube[] cubes)
    {
        IEnumerable<Rigidbody> separatableCubes = cubes
                .Select(cube => cube.Explodable.RigidBody);

        foreach (Rigidbody separatable in separatableCubes)
            separatable.AddExplosionForce(SeparationForce,
                transform.position, SeparationRadius);
    }
}