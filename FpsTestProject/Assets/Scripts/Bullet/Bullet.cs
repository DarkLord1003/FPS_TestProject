using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [Range(5, 10)]
    [SerializeField] private float _destroyAfter;
    [SerializeField] private float _minDestroyTime;
    [SerializeField] private float _maxDestroyTime;
    [SerializeField] private bool _destroyOnImpact;

    private void OnCollisionEnter(Collision other)
    {
        if (!_destroyOnImpact)
        {
            DestroyTimer();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(Random.Range(_minDestroyTime, _maxDestroyTime));
        gameObject.SetActive(false);
    }
}
