using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCasing : MonoBehaviour
{
    [Header("Casing Spawn Settings")]
    [SerializeField] private PoolComponent _casingPrefab;
    [SerializeField] private Transform _casingSpawnPoint;

    [Header("Speed Rotation")]
    [SerializeField] private float _speedThrow;
    private void Start()
    {
        ObjectPool.CreatePool(_casingPrefab, "Casing", 10, false);
    }
    private void Spawn()
    {
        PoolComponent casing = ObjectPool.GetObject("Casing", _casingSpawnPoint.position, _casingSpawnPoint.rotation);

        casing.GetComponent<Rigidbody>().velocity = transform.right * _speedThrow * Time.deltaTime;
    }
}
