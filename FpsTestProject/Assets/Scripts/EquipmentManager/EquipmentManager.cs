using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour,IListener
{
    [Header("Input Manager")]
    [SerializeField] private InputManager _inputManager;

    [Header("Animator")]
    [SerializeField] private Animator _playerAnimator;

    [Header("Inventory")]
    [SerializeField] private Inventory _inventory;

    [Header("Hands")]
    [SerializeField] private Weapon[] _weapons;

    [Header("Parent")]
    [SerializeField] private Transform _parent;

    public EquipmentManager EquipedManager => this;

    public Weapon CurrentEqupedWeapon => _currentEqupedWeapon;
 
    public Weapon NextEqupedWeapon => _nextEqupedWeapon;
   
    public Gun CurrentEqupedGun => _currentEquipedGun;

    private Weapon _currentEqupedWeapon;
    private Gun _currentEquipedGun;
    private Weapon _nextEqupedWeapon;
    private Item item;
    private int _currentlyWeaponStyle = 2;
    private int _state = 0;
    private bool _canEqupedWeapon;

    private void Start()
    {
        if (_state == 0)
        {
            EventManager.Instance.AddListener(Event_Type.Equiped_Weapon, this);
        }

        _canEqupedWeapon = true;
    }

    private void Update()
    {
        if (_inputManager.Alpha1IsTrigger && _canEqupedWeapon)
        {
            if (_currentlyWeaponStyle != 0)
            {
                item = _inventory.GetItem(0);

                if (item == null)
                    return;

                EquipedWeapon(item);
                UniquipedWeapon();
                SetAnimationState(item);
                StartCoroutine(DelayWeaponEquiped());
            }  
        }

        if (_inputManager.Alpha2IsTrigger && _canEqupedWeapon)
        {
            if (_currentlyWeaponStyle != 1)
            {
                item = _inventory.GetItem(1);

                if (item == null)
                    return;

                EquipedWeapon(item);
                UniquipedWeapon();
                SetAnimationState(item);
                StartCoroutine(DelayWeaponEquiped());
            }
        }

        if (_inputManager.Alpha3IsTrigger && _canEqupedWeapon)
        {
            if (_currentlyWeaponStyle != 2)
            {
                item = _inventory.GetItem(2);

                if (item == null)
                    return;

                EquipedWeapon(item);
                UniquipedWeapon();
                SetAnimationState(item);
                StartCoroutine(DelayWeaponEquiped());
            }
        }

    }


    #region - General Methods -

    private void EquipedWeapon(Item item)
    {
        Gun gun = item as Gun;

        if (gun == null)
        {
            UniquipedWeapon();
            SetAnimationState(gun);
            return;
        }
            

        for(int i = 0; i < _weapons.Length; i++)
        {
            if(_weapons[i].NameWeapon.ToLower() == gun.Name.ToLower())
            {
                _currentEquipedGun = gun;
                _currentEqupedWeapon = _nextEqupedWeapon;
                _nextEqupedWeapon = _weapons[i];
                Debug.Log(_nextEqupedWeapon.name);
            }
        }    
    }

    private void EqupedHands()
    {
        foreach(var weapon in _weapons)
        {
            weapon.gameObject.SetActive(false);
        }
    }
    private void UniquipedWeapon()
    {
        _playerAnimator.SetTrigger("UniquipWeapon");
    }

    private void SetAnimationState(Item item)
    {
        Gun gun = item as Gun;

        if (gun)
        {
            _currentlyWeaponStyle = (int)gun.WeaponStyle;
            _playerAnimator.SetInteger("CurrentWeapon", gun.ID);
        }

    }
    #endregion

    #region - IListener -

    public void OnEvent(Event_Type eventType, Component sender, Object param = null)
    {
        if (eventType == Event_Type.Equiped_Weapon)
        {
            Gun gun = param as Gun;

            if (gun)
            {
                UniquipedWeapon();
                EquipedWeapon(gun);
                SetAnimationState(gun);
            }          
        }
    }

    #endregion

    #region - Coroutines -

    private IEnumerator DelayWeaponEquiped()
    {
        _canEqupedWeapon = false;
        yield return new WaitForSeconds(2f);
        _canEqupedWeapon = true;

        yield break;
    }

    #endregion

    #region - OnDisable/Enable - 

    private void OnEnable()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.AddListener(Event_Type.Equiped_Weapon, this);
            _state = 1;
        }
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(Event_Type.Equiped_Weapon, this);
    }
    #endregion
}
