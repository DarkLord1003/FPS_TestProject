using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCasing : MonoBehaviour,IListener
{
    [Header("Casing Spawn Settings")]
    [SerializeField] private PoolComponent _casingPrefab;
    [SerializeField] private Transform _casingSpawnPoint;

    [Header("Speed Throw")]
    [SerializeField] private float _speedThrow;


    private void Start()
    {
        EventManager.Instance.AddListener(Event_Type.Spawn_Casing, this);
        ObjectPool.CreatePool(_casingPrefab, "Casing", 10, false);
    }

    #region - Spawn -
    private void Spawn()
    {
        PoolComponent casing = ObjectPool.GetObject("Casing", _casingSpawnPoint.position, _casingSpawnPoint.rotation);

        casing.GetComponent<Rigidbody>().velocity = transform.right * _speedThrow * Time.deltaTime;
    }

    #endregion

    #region - OnDisable -
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(this);
    }

    #endregion

    #region - OnEvent -
    public void OnEvent(Event_Type eventType, Component sender, Object param = null)
    {
        Spawn();
    }

    #endregion

}
