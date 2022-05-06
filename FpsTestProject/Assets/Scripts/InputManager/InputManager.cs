using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput _playerInput;

    #region - Actions -

    private InputAction _move;
    private InputAction _view;
    private InputAction _jump;
    private InputAction _sprintPress;
    private InputAction _sprintRealised;
    private InputAction _crouch;
    private InputAction _prone;
    private InputAction _aimingPress;
    private InputAction _aimingRealised;
    private InputAction _shoot;
    private InputAction _reload;
    private InputAction _aim;
    private InputAction _shootRealised;
    private InputAction _changeShootType;
    private InputAction _alpha1;
    private InputAction _alpha2;
    private InputAction _alpha3;
    private InputAction _alpha4;
    private InputAction _use;
    private InputAction _pickupItem;
    private InputAction _throw;
    private InputAction _useHands;
    private InputAction _openInventory;

    #endregion

    #region - Move and view data

    [Header("Player Move and View")]
    private Vector2 _moveInput;
    private Vector2 _viewInput;

    #endregion

    #region - Private bool fields

    private bool _jumpIsTrigger;
    private bool _sprintPressIsTrigger;
    private bool _sprintRealisedIsTrigger;
    private bool _aimingPressTrigger;
    private bool _aimingRealisedTrigger;
    private bool _isCrouching;
    private bool _isProning;
    private bool _shootIsTrigger;
    private bool _shootRealisedTrigger;
    private bool _reloadIsTrigger;
    private bool _aimIsTrigger;
    private bool _changeShootTypeIsTrigger;
    private bool _alpha1IsTrigger;
    private bool _alpha2IsTrigger;
    private bool _alpha3IsTrigger;
    private bool _alpha4IsTrigger;
    private bool _useIsTrigger;
    private bool _pickupItemIsTrigger;
    private bool _throwIsTrigger;
    private bool _useHandsIsTrigger;
    private bool _openInventoryIsTrigger;

    #endregion

    #region - Public Properties
    public Vector2 MoveInput
    {
        get => _moveInput;
    }

    public Vector2 ViewInput
    {
        get => _viewInput;
    }

    public bool JumpIsTrigger
    {
        get => _jumpIsTrigger;
    }

    public bool SprintPressIsTrigger
    {
        get => _sprintPressIsTrigger;
    }

    public bool SprintRelisedIsTrigger
    {
        get => _sprintRealisedIsTrigger;
    }

    public bool IsCrouching
    {
        get => _isCrouching;
    }

    public bool IsProning
    {
        get => _isProning;
    }

    public bool AimingPressTrigger
    {
        get => _aimingPressTrigger;
    }

    public bool AimingRealisedTrigger
    {
        get => _aimingRealisedTrigger;
    }

    public bool ShootIsTrigger
    {
        get => _shootIsTrigger;
    }

    public bool ShootRealisedTrigger
    {
        get => _shootRealisedTrigger;
    }
    public bool ReloadIsTrigger
    {
        get => _reloadIsTrigger;
    }

    public bool AimIsTrigger
    {
        get => _aimIsTrigger;
    }

    public bool ChangeShootTypeIsTrigger
    {
        get => _changeShootTypeIsTrigger;
    }

    public bool Alpha1IsTrigger
    {
        get => _alpha1IsTrigger;
    }

    public bool Alpha2IsTrigger
    {
        get => _alpha2IsTrigger;
    }

    public bool Alpha3IsTrigger
    {
        get => _alpha3IsTrigger;
    }

    public bool Alpha4IsTrigger
    {
        get => _alpha4IsTrigger;
    }

    public bool UseIsTrigger
    {
        get => _useIsTrigger;
    }

    public bool PickupItemIsTrigger
    {
        get => _pickupItemIsTrigger;
    }

    public bool TrowIsTrigger
    {
        get => _throwIsTrigger;
    }

    public bool UseHandsIsTrigger
    {
        get => _useHandsIsTrigger;
    }

    public bool OpenInventoryIsTrigger
    {
        get => _openInventoryIsTrigger;
    }

    public InputAction ShootAction => _shoot;

    #endregion


    private void Awake()
    {
        GetActions();
    }

    private void Update()
    {
        GetValue();
    }

    #region - GetValue -

    private void GetValue()
    {
        _moveInput = _move.ReadValue<Vector2>();
        _viewInput = _view.ReadValue<Vector2>();

        _jumpIsTrigger = _jump.triggered;
        _sprintPressIsTrigger = _sprintPress.triggered;
        _sprintRealisedIsTrigger = _sprintRealised.triggered;

        _isCrouching = _crouch.triggered;
        _isProning = _prone.triggered;

        _aimingPressTrigger = _aimingPress.triggered;
        _aimingRealisedTrigger = _aimingRealised.triggered;
        _aimIsTrigger = _aim.triggered;

        _shootIsTrigger = _shoot.triggered;
        _shootRealisedTrigger = _shootRealised.triggered;
        _reloadIsTrigger = _reload.triggered;

        _changeShootTypeIsTrigger = _changeShootType.triggered;

        _alpha1IsTrigger = _alpha1.triggered;
        _alpha2IsTrigger = _alpha2.triggered;
        _alpha3IsTrigger = _alpha3.triggered;
        _alpha4IsTrigger = _alpha4.triggered;

        _useIsTrigger = _use.triggered;
        _pickupItemIsTrigger = _pickupItem.triggered;
        _throwIsTrigger = _throw.triggered;
        _useHandsIsTrigger = _useHands.triggered;

        _openInventoryIsTrigger = _openInventory.triggered;

    }

    #endregion

    #region - GetActions -

    private void GetActions()
    {
        _move = _playerInput.actions["Movement"];
        _view = _playerInput.actions["View"];
        _jump = _playerInput.actions["Jump"];
        _sprintPress = _playerInput.actions["SprintPress"];
        _sprintRealised = _playerInput.actions["SprintRealised"];
        _crouch = _playerInput.actions["Crouch"];
        _prone = _playerInput.actions["Prone"];
        _aimingPress = _playerInput.actions["AimingPress"];
        _aimingRealised = _playerInput.actions["AimingRealised"];
        _shoot = _playerInput.actions["Shoot"];
        _shootRealised = _playerInput.actions["ShootRealised"];
        _reload = _playerInput.actions["Reload"];
        _aim = _playerInput.actions["Aim"];
        _changeShootType = _playerInput.actions["ChangeShootType"];
        _alpha1 = _playerInput.actions["Alpha1"];
        _alpha2 = _playerInput.actions["Alpha2"];
        _alpha3 = _playerInput.actions["Alpha3"];
        _alpha4 = _playerInput.actions["Alpha4"];
        _use = _playerInput.actions["Use"];
        _pickupItem = _playerInput.actions["PickupItem"];
        _throw = _playerInput.actions["Throw"];
        _useHands = _playerInput.actions["UseHands"];
        _openInventory = _playerInput.actions["OpenInventory"];
    }

    #endregion

}
