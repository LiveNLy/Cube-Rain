using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Cube _cube;

    private int _lifeTime;
    private int _timesColorChanged = 0;
    private Coroutine _coroutine;

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

    private void Start()
    {
        int minLifeTime = 2;
        int maxLifeTime = 5;
        _lifeTime = UnityEngine.Random.Range(minLifeTime, maxLifeTime + 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out MeshCollider platform) && _timesColorChanged == 0)
        {
            _timesColorChanged++;
            _cube.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
            _coroutine = StartCoroutine(Timer());
        }
    }

    private IEnumerator Timer()
    {
        var wait = new WaitForSeconds(1);

        for (int i = _lifeTime; i > 0; i--)
        {
            yield return wait;
        }

        Destroy(gameObject);
    }
}
