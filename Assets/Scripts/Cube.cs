using UnityEngine;

[RequireComponent(typeof(RandomColorChanger), typeof(Explodable))]
public class Cube : MonoBehaviour
{
    [SerializeField] private float _startSeparationChance = 1f;

    private float _separationChance;
    private RandomColorChanger _randomColorChanger;

    public Explodable Explodable { get; private set; }

    private void Awake()
    {
        Explodable = GetComponent<Explodable>();

        _randomColorChanger = GetComponent<RandomColorChanger>();

        _separationChance = _startSeparationChance;
    }

    private void OnMouseUpAsButton()
    {
        if (_separationChance > Random.Range(0, 1f))
            Explodable.Separate(SpawnCubes());
        else
            Explodable.Explode();

        Destroy(gameObject);
    }

    public void Init(Vector3 prevScale, float prevSeparationChance)
    {
        transform.localScale = prevScale / 2;
        _separationChance = prevSeparationChance / 2;

        _randomColorChanger.Change();
    }

    private Cube[] SpawnCubes()
    {
        int newCubesCount = Random.Range(2, 7);

        Cube[] cubes = new Cube[newCubesCount];

        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i] = Instantiate(this, transform.parent);

            cubes[i].Init(transform.localScale, _separationChance);
            cubes[i].Explodable.Init(Explodable);
        }

        return cubes;
    }
}