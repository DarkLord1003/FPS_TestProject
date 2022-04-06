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

    private int _state;


    private void Start()
    {
        if (_state == 0)
        {
            EventManager.Instance.AddListener(Event_Type.Spawn_Casing, this);
        }

        ObjectPool.CreatePool(_casingPrefab, gameObject.name + "Casing", 20, true);
    }

    #region - Spawn -
    private void Spawn()
    {
        PoolComponent casing = ObjectPool.GetObject(gameObject.name + "Casing", _casingSpawnPoint.position, _casingSpawnPoint.rotation);

        casing.GetComponent<Rigidbody>().velocity = transform.right * _speedThrow * Time.deltaTime;
    }

    #endregion

    #region - OnDisable/Enable -
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(Event_Type.Spawn_Casing,this);
    }

    private void OnEnable()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.AddListener(Event_Type.Spawn_Casing, this);
            _state = 1;
        }
    }

    #endregion

    #region - OnEvent -
    public void OnEvent(Event_Type eventType, Component sender, Object param = null)
    {
        Spawn();
    }

    #endregion

}
