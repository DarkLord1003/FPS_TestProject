using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour,IListener
{
    [Header("Input Manager")]
    [SerializeField] private InputManager _inputManager;

    [Header("Animator")]
    [SerializeField] private Animator _playerAnimator;

    [Header("Hands")]
    [SerializeField] private Weapon[] _weapons;

    public EquipmentManager EquipedManager
    {
        get => this;
    }

    public Weapon CurrentEqupedWeapon
    {
        get => _currentEqupedWeapon;
    }

    public Weapon NextEqupedWeapon
    {
        get => _nextEqupedWeapon;
    }

    private Weapon _currentEqupedWeapon;
    private Weapon _nextEqupedWeapon;
    private Inventory _inventory;
    private int _currentlyWeaponStyle = 2;
    private int _state = 0;

    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        if (_state == 0)
        {
            EventManager.Instance.AddListener(Event_Type.Equiped_Weapon, this);
        }
    }

    private void Update()
    {
        if (_inputManager.Alpha1IsTrigger)
        {
            if (_currentlyWeaponStyle != 0)
            {
                EquipedWeapon(_inventory.GetItem(0));
                UniquipedWeapon();
                SetAnimationState(_inventory.GetItem(0));
            }  
        }

        if (_inputManager.Alpha2IsTrigger)
        {
            if (_currentlyWeaponStyle != 1)
            {
                EquipedWeapon(_inventory.GetItem(1));
                UniquipedWeapon();
                SetAnimationState(_inventory.GetItem(1));
            }
        }

        if (_inputManager.Alpha4IsTrigger)
        {
            if (_currentlyWeaponStyle != 2)
            {
                UniquipedWeapon();
                SetAnimationState(_inventory.GetItem(3));
                EqupedHands();
                _currentlyWeaponStyle = 2;
            }
        }
    }


    #region - General Methods -

    private void EquipedWeapon(Item item)
    {
        Gun gun = item as Gun;

        if (gun == null)
            return;

        for(int i = 0; i < _weapons.Length; i++)
        {
            if(_weapons[i].NameWeapon.ToLower() == gun.Name.ToLower())
            {
                _currentEqupedWeapon = _nextEqupedWeapon;
                _nextEqupedWeapon = _weapons[i];
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
