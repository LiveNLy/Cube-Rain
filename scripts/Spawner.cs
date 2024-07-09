using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Spawner _spawnPosition;
    [SerializeField] private Color _cubeOriginaleColor;
    [SerializeField] private float _repeatRate = 0.5f;
    [SerializeField] private int _poolDefaultCapacity = 5;
    [SerializeField] private int _poolMaxSize = 12;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            actionOnGet: (cube) => SetCube(cube),
            createFunc: () => Instantiate(_cubePrefab),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnCube(_repeatRate));
    }

    public void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
        cube.ResetCube(_cubeOriginaleColor);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void SetCube(Cube cube)
    {
        cube.transform.position = SetRandomPosition();
        cube.gameObject.SetActive(true);
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

    private IEnumerator SpawnCube(float seconds)
    {
        var wait = new WaitForSeconds(seconds);

        while (enabled)
        {
            GetCube();
            yield return wait;
        }
    }
}
