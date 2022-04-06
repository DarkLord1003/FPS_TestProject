using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [Header("Input Manager")]
    [SerializeField] private InputManager _inputManager;

    [Header("Animator")]
    [SerializeField] private Animator _playerAnimator;

    [Header("Hands")]
    [SerializeField] private Weapon[] _weapons;

    private Weapon _currentWeapon;
    private Inventory _inventory;

    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        if (_inputManager.Alpha1IsTrigger)
        {
            UniquipedWeapon();
            SetAnimationState(_inventory.GetItem(0));
            EquipedWeapno(_inventory.GetItem(0));
        }

        if (_inputManager.Alpha2IsTrigger)
        {
            SetAnimationState(_inventory.GetItem(1));
            EquipedWeapno(_inventory.GetItem(1));
        }

        if (_inputManager.Alpha3IsTrigger)
        {
            SetAnimationState(_inventory.GetItem(2));
            EquipedWeapno(_inventory.GetItem(2));
        }
    }


    private void EquipedWeapno(Item item)
    {
        Gun gun = item as Gun;

        if (gun == null)
            return;

        for(int i = 0; i < _weapons.Length; i++)
        {
            if(_weapons[i].NameWeapon.ToLower() == gun.Name.ToLower())
            {
                _weapons[i].gameObject.SetActive(true);
                _currentWeapon = _weapons[i];
            }
            else
            {
                _weapons[i].gameObject.SetActive(false);
            }
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
            _playerAnimator.SetInteger("CurrentWeapon", (int)gun.WeaponType);
        }

    }
}
