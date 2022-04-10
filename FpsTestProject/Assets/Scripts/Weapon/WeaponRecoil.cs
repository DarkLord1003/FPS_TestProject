using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour,IListener
{

    [Header("Transform")]
    [SerializeField] private Transform _recoilTransform;

    [Header("Recoil Type")]
    [SerializeField] private Recoil_Type _recoilType;

    [Header("Settings Random Recoil")]
    [SerializeField,Range(0,30)] private float _recoilX;
    [SerializeField,Range(0,30)] private float _recoilY;
    [SerializeField,Range(0,30)] private float _recoilZ;

    [Header("Settings Pattern Recoil")]
    [SerializeField] private Vector2[] _pattern;

    [Header("Delay")]
    [SerializeField] private float _snapiness;
    [SerializeField] private float _returnSpeed;

    private int index = 0;
    private int _state = 0;
    private Vector3 _currentRotation;
    private Vector3 _targetRotation;

    private void Start()
    {
        if (_state == 0)
        {
            EventManager.Instance.AddListener(Event_Type.Weapon_Recoil, this);
        }
    }
    private void Update()
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, _snapiness * Time.deltaTime);
        _recoilTransform.localRotation = Quaternion.Euler(_currentRotation);
    }

    private void Recoil()
    {
        if (_recoilType == Recoil_Type.Random)
        {
            _targetRotation += new Vector3(_recoilX, Random.Range(-_recoilY, _recoilY), Random.Range(-_recoilZ, _recoilZ));
        }
        else if(_recoilType == Recoil_Type.Pattern)
        {
            _targetRotation += new Vector3(_pattern[index].x, _pattern[index].y, 0f);

            if (index < _pattern.Length-1)
                index++;
            else
                index = 0;
        }
    }

   
    public void OnEvent(Event_Type eventType, Component sender, Object param = null)
    {
        Recoil();
    }


    #region - Recoil Type -
    enum Recoil_Type
    {
        Random,
        Pattern,
        None
    }

    #endregion


    #region - OnDisable/Enable -

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(Event_Type.Weapon_Recoil,this);
    }

    private void OnEnable()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.AddListener(Event_Type.Weapon_Recoil, this);
            _state = 1;
        }
    }


    #endregion

}
