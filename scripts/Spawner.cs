using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private GameObject _spawnPosition;
    [SerializeField] private float _repeatRate = 0.5f;
    [SerializeField] private int _poolDefaultCapacity = 12;
    [SerializeField] private int _poolMaxSize = 12;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cube),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.SetActive(false),
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = SetRandomPosition();
        cube.SetActive(true);
    }

    private Vector3 SetRandomPosition()
    {
        Vector3 position = _spawnPosition.transform.position;
        float minRandom = -15f;
        float maxRandom = 15f;

        position.x = _spawnPosition.transform.position.x - Random.Range(minRandom, maxRandom);
        position.z = _spawnPosition.transform.position.z - Random.Range(minRandom, maxRandom);

        return position;
    }

    private void OnTriggerEnter(Collider other)
    {
        _pool.Release(_cube);
    }
}
