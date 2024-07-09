using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;

    private int _lifeTime;
    private int _timesColorChanged = 0;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out MeshCollider platform) && _timesColorChanged == 0)
        {
            _timesColorChanged++;
            _renderer.material.color = Random.ColorHSV();
            StartCoroutine(CountLifeTime());
        }
    }

    public void ResetCube(Color color)
    {
        _renderer.material.color = color;
        _timesColorChanged = 0;
    }

    private IEnumerator CountLifeTime()
    {
        int minLifeTime = 2;
        int maxLifeTime = 5;
        _lifeTime = Random.Range(minLifeTime, maxLifeTime + 1);

        yield return new WaitForSeconds(_lifeTime);

        _spawner.ReleaseCube(this);
    }
}
