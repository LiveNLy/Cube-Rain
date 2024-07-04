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

    public void ResetCube(Color color)
    {
        SetTime();
        _renderer.material.color = color;
        _timesColorChanged = 0;
    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        SetTime();
    }

    private void SetTime()
    {
        int minLifeTime = 2;
        int maxLifeTime = 5;
        _lifeTime = Random.Range(minLifeTime, maxLifeTime + 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out MeshCollider platform) && _timesColorChanged == 0)
        {
            _timesColorChanged++;
            _renderer.material.color = Random.ColorHSV();
            StartCoroutine(Timer());
        }
    }

    private IEnumerator Timer()
    {
        var wait = new WaitForSeconds(1);

        while (_lifeTime != 0)
        {
            _lifeTime--;
            yield return wait;
        }

        _spawner.ReleaseCube(this);
    }
}
