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

    [Header("Actions Player")]
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

    [Header("Player Move and View")]
    private Vector2 _moveInput;
    private Vector2 _viewInput;


    private bool _jumpIsTrigger;
    private bool _sprintPressIsTrigger;
    private bool _sprintRealisedIsTrigger;
    private bool _aimingPressTrigger;
    private bool _aimingRealisedTrigger;
    private bool _isCrouching;
    private bool _isProning;
    private bool _shootIsTrigger;
    private bool _reloadIsTrigger;
    private bool _aimIsTrigger;
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

    public bool ReloadIsTrigger
    {
        get => _reloadIsTrigger;
    }

    public bool AimIsTrigger
    {
        get => _aimIsTrigger;
    }

    private void Awake()
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
        _reload = _playerInput.actions["Reload"];
        _aim = _playerInput.actions["Aim"];
    }

    private void Update()
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
        _reloadIsTrigger = _reload.triggered;
    }

    #region - Enable/Disable

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    #endregion
}
