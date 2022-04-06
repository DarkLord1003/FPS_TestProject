using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ModelSettings;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform _camera;

    [Header("Input Manager")]
    [SerializeField] private InputManager _playerInput;

    [Header("Settings")]
    [SerializeField] private PlayerSettings _playerSettings;

    [Header("References")]
    [SerializeField] private Transform _feet;
    private CharacterController _characterController;
    private Animator _animator;

    [Header("Gravity")]
    [SerializeField] private float _playerGravity;

    [Header("Sprinting")]
    private bool _isSprinting;

    [Header("Check Ground")]
    [SerializeField] private float _radiusSphere;
    [SerializeField] private LayerMask _groundMask;
    private bool _isGround;

    [Header("Player Stance")]
    [SerializeField] private PlayerStance _stand;
    [SerializeField] private PlayerStance _crouch;
    [SerializeField] private PlayerStance _prone;
    [SerializeField] private LayerMask _cellingCheckMask;
    [SerializeField] private float _checkCellingErrorMargin = 0.05f;
    private PlayerState _currentState;
    private PlayerStance _currentStance;

    [Header("Speed Effector")]
    private float _currentSpeedEffector;

    private Vector3 _newCharacterPosition;
    private Vector3 _newCharacterRotation;
    private Vector3 _newCameraRotation;
    private Vector3 _newSmoothingCharacterPosition;
    private Vector3 _MoveSmoothingVelocity;

    private Vector3 _newCharacterRotationSmooth;
    private Vector3 _newCameraRotationSmooth;
    private Vector3 _newCharacterRotationSmoothVelocity;
    private Vector3 _newCameraRotationSmoothVelocity;


    private Vector3 _jumpSmoothVelocity;
    private Vector3 _playerJumpHeight;

    private Vector3 _centerCharacterVelocity;
    private float _heightCharacterVelocity;
    private float _cameraSmoothingVelocity;
    private float _cameraHeight;
    

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        CalculateMove();
        CalculateView();
        CalculateJump();
        CalculateStance();
        CalculateAnimation();
        ChechGround();
        Crouch();
        Prone();
        Jump();
        Sprint();
    }


    #region - Movement - 

    private void CalculateMove()
    {
        Vector2 moveInput = _playerInput.MoveInput;

        if (moveInput.y < 0.25f)
            _isSprinting = false;


        float horizontalDir = _playerSettings.StrafeSpeed;
        float verticalDir = _playerSettings.ForwardSpeed;

        if (_isSprinting)
        {
            horizontalDir = _playerSettings.SprintStrafeSpeed;
            verticalDir = _playerSettings.SprintForwardSpeed;
        }

        if(_currentState == PlayerState.Crouch)
        {
            _currentSpeedEffector = _playerSettings.SpeedEffectorCrouch;
        }
        else if(_currentState == PlayerState.Prone)
        {
            _currentSpeedEffector = _playerSettings.SpeedEffectorProne;
        }
        else if (!_isGround)
        {
            _currentSpeedEffector = _playerSettings.SpeedEffectorFalling;
        }
        else
        {
            _currentSpeedEffector = _playerSettings.SpeedEffector;
        }

        horizontalDir *= _currentSpeedEffector;
        verticalDir *= _currentSpeedEffector;

        _newCharacterPosition = ((transform.forward * verticalDir * moveInput.y) 
                                + (transform.right * horizontalDir * moveInput.x)) * Time.deltaTime;

        _newSmoothingCharacterPosition = Vector3.SmoothDamp(_newSmoothingCharacterPosition, _newCharacterPosition,
                                         ref _MoveSmoothingVelocity, _playerSettings.SmoothMoveSpeed);

        if(!_isGround && _playerGravity > _playerSettings.MinGravity)
        {
            _playerGravity += (-9.8f * _playerSettings.GravityAmmount * Time.deltaTime) * Time.deltaTime;
        }

        if (_isGround)
        {
            _playerGravity = -0.01f;
        }

        _newSmoothingCharacterPosition.y = _playerGravity;
        _newSmoothingCharacterPosition += _playerJumpHeight * Time.deltaTime;
        _characterController.Move(_newSmoothingCharacterPosition);
    }

    #endregion

    #region - View -

    private void CalculateView()
    {
        Vector2 viewInput = _playerInput.ViewInput;

        _newCameraRotation.x += _playerSettings.SensivitivytiY * (_playerSettings.InverseY ? viewInput.y : -viewInput.y)
                                                               * Time.deltaTime;

        _newCameraRotation.x = Mathf.Clamp(_newCameraRotation.x, -_playerSettings.ClampX, _playerSettings.ClampX);

        _newCharacterRotation.y += _playerSettings.SensivitivytiX * (_playerSettings.InverseX ? -viewInput.x : viewInput.x)
                                                                  * Time.deltaTime;

        _newCameraRotationSmooth = Vector3.SmoothDamp(_newCameraRotationSmooth, _newCameraRotation,
          ref _newCameraRotationSmoothVelocity, _playerSettings.ViewSmoothSpeed);

        _newCharacterRotationSmooth = Vector3.SmoothDamp(_newCharacterRotationSmooth, _newCharacterRotation,
          ref _newCharacterRotationSmoothVelocity, _playerSettings.ViewSmoothSpeed);

        _camera.localRotation = Quaternion.Euler(_newCameraRotationSmooth);
        transform.localRotation = Quaternion.Euler(_newCharacterRotationSmooth);
    }

    #endregion

    #region - Jump -

    private void Jump()
    {
        if (_playerInput.JumpIsTrigger && _currentState != PlayerState.Crouch &&
                                           _currentState != PlayerState.Prone)
        {
            if (!_isGround)
            {
                return;
            }

            _playerGravity = 0;
            _playerJumpHeight = new Vector3(0f, _playerSettings.JumpHeight, 0f);
        }
        else if(_playerInput.JumpIsTrigger && (_currentState == PlayerState.Crouch ||
                                               _currentState == PlayerState.Prone))
        {
            if (CheckCelling(_stand.Collider.height))
                return;

            _currentState = PlayerState.Stand;
            return;
        }
                                               
    }

    private void CalculateJump()
    {
        _playerJumpHeight = Vector3.SmoothDamp(_playerJumpHeight, Vector3.zero,
                            ref _jumpSmoothVelocity, _playerSettings.JumpSmoothTime);
    }

    #endregion

    #region - Sprint -

    private void Sprint()
    {
        if (_playerInput.SprintPressIsTrigger && _currentState != PlayerState.Crouch && 
                                                 _currentState != PlayerState.Prone)
        {
            _isSprinting = true;
        }
        else if(_playerInput.SprintRelisedIsTrigger)
        {
            _isSprinting = false;
        }
    }

    #endregion

    #region - Stance -

    private void CalculateStance()
    {
        _currentStance = _stand;

        if(_currentState == PlayerState.Crouch)
        {
            _currentStance = _crouch;
        }

        if(_currentState == PlayerState.Prone)
        {
            _currentStance = _prone;
        }

        _cameraHeight = Mathf.SmoothDamp(_cameraHeight, _currentStance.CameraHeight,
                        ref _cameraSmoothingVelocity, _playerSettings.CameraSmoothingSpeed);

        _camera.localPosition = new Vector3(_camera.localPosition.x, _cameraHeight, _camera.localPosition.z);

        _characterController.height = Mathf.SmoothDamp(_characterController.height, _currentStance.Collider.height,
                                      ref _heightCharacterVelocity, _playerSettings.HeightCharacterSmoothingSpeed);

        _characterController.center = Vector3.SmoothDamp(_characterController.center, _currentStance.Collider.center,
                                      ref _centerCharacterVelocity, _playerSettings.CenterCharacterSmoothingSpeed);
    }

    private void Crouch()
    {
        if (_playerInput.IsCrouching)
        {
            if (_currentState == PlayerState.Crouch)
            {
                if (CheckCelling(_stand.Collider.height))
                {
                    return;
                }

                _currentState = PlayerState.Stand;
                return;
            }

            _currentState = PlayerState.Crouch;
        }
    }

    private void Prone()
    {
        if (_playerInput.IsProning)
        {
            if (_currentState == PlayerState.Prone)
            {
                if (CheckCelling(_stand.Collider.height))
                {
                    return;
                }

                _currentState = PlayerState.Stand;
                return;
            }

            _currentState = PlayerState.Prone;
        }
    }

    #endregion

    #region - Check Ground -

    private void ChechGround()
    {
        _isGround = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), _radiusSphere, _groundMask);
    }

    #endregion

    #region - Check Celling -

    private bool CheckCelling(float heightStance)
    {
        Vector3 start = new Vector3(_feet.transform.position.x, _feet.transform.position.y +
                        _characterController.radius + _checkCellingErrorMargin, _feet.transform.position.z);

        Vector3 end = new Vector3(_feet.transform.position.x, _feet.transform.position.y -
                      _characterController.radius - _checkCellingErrorMargin + heightStance,_feet.transform.position.z);

        return Physics.CheckCapsule(start, end, _characterController.radius,_cellingCheckMask);
    }

    #endregion

    #region - Animation -

    private void CalculateAnimation()
    {
        _animator.SetFloat("Speed", _characterController.velocity.magnitude);
    }

    #endregion

    #region - Gizmos -

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 1, 0), _radiusSphere);

        Vector3 start = new Vector3(_feet.transform.position.x, _feet.transform.position.y +
                        0.5f + _checkCellingErrorMargin, _feet.transform.position.z);

        Vector3 end = new Vector3(_feet.transform.position.x, _feet.transform.position.y -
                      0.5f - _checkCellingErrorMargin + _stand.Collider.height,_feet.transform.position.z);

        Gizmos.DrawWireSphere(start, 0.5f);
        Gizmos.DrawWireSphere(end, 0.5f);
    }

    #endregion

}
