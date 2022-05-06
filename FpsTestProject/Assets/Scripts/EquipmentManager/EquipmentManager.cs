using System.Collections;
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

    public Weapon CurrentEqupedWeapon
    {
        get=> _currentEqupedWeapon;
        set => _currentEqupedWeapon = value;
    }
 
    public Weapon NextEqupedWeapon
    {
        get => _nextEqupedWeapon;
        set => _nextEqupedWeapon = value;
    }
   
    public Gun CurrentEqupedGun => _currentEquipedGun;
    public Animator PlayerAnimator => _playerAnimator;
    public Weapon[] Weapons => _weapons;

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
            Eqiped(0);
        }

        if (_inputManager.Alpha2IsTrigger && _canEqupedWeapon)
        {
            Eqiped(1);
        }

        if (_inputManager.Alpha3IsTrigger && _canEqupedWeapon)
        {
            Eqiped(2);
        }

        if (_inputManager.TrowIsTrigger)
        {
            if (_currentEquipedGun)
            {
                UniquipedWeapon();
                _currentlyWeaponStyle = 2;
                _inventory.RemoveItem(_currentEquipedGun);
                _playerAnimator.SetInteger("CurrentWeapon", 0);
                StartCoroutine(DelayTrhow(_currentEquipedGun));
            }
        }

        if (_inputManager.UseHandsIsTrigger)
        {
            if (_currentlyWeaponStyle != 2)
            {
                UniquipedWeapon();
                _currentlyWeaponStyle = 2;
                StartCoroutine(DelayEqupedHands());
                _playerAnimator.SetInteger("CurrentWeapon", 0);
            }
        }

    }


    #region - General Methods -

    public void Eqiped(int slotIndex)
    {
        if (_currentlyWeaponStyle != 1)
        {
            item = _inventory.GetItem(slotIndex);

            if (item == null)
                return;

            EquipedWeapon(item);
            UniquipedWeapon();
            SetAnimationState(item);
            StartCoroutine(DelayWeaponEquiped());
        }
    }
    private void EquipedWeapon(Item item)
    {
        Gun gun = item as Gun;

        if (gun == null)
        {
            return;
        }
            

        for(int i = 0; i < _weapons.Length; i++)
        {
            if(_weapons[i].NameWeapon.ToLower() == gun.Name.ToLower())
            {
                _currentEquipedGun = gun;
                _currentEqupedWeapon = _nextEqupedWeapon;
                _nextEqupedWeapon = _weapons[i];
            }
        }    
    }

    public void UniquipedWeapon()
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

    public void SetAnimationState(int state)
    {
        _playerAnimator.SetInteger("CurrentWeapon", state);
    }

    public Weapon GetWeapon(Gun gun)
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            if (_weapons[i].NameWeapon.ToLower() == gun.Name.ToLower())
            {
                return _weapons[i];
            }
        }

        return null;
    }
    private void DeactivateWeapon()
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.gameObject.SetActive(false);
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

    private IEnumerator DelayTrhow(Item item)
    {
        yield return new WaitForSeconds(0.4f);

        if (item)
        {
            DeactivateWeapon();

            _currentEquipedGun = null;
            _currentEqupedWeapon = null;
            _inventory.ThrowItem(item);
        }
    }

    private IEnumerator DelayEqupedHands()
    {
        yield return new WaitForSeconds(0.5f);
        DeactivateWeapon();
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
