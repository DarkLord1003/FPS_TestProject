using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class ModelSettings
{

    #region - Player -

    public enum PlayerState
    {
        Stand,
        Crouch,
        Prone
    }

    [Serializable]
    public class PlayerStance
    {
        [SerializeField] private float _cameraHeight;
        [SerializeField] private float _height;
        [SerializeField] private Vector3 _center;

        public float CameraHeight => _cameraHeight;
        public float Height => _height;
        public Vector3 Center => _center;
    }


    [Serializable]
    public class PlayerSettings
    {
        [Header("Move")]
        [SerializeField] private float _forwardSpeed;
        [SerializeField] private float _strafeSpeed;
        [SerializeField] private float _smoothMoveSpeed;
        public float ForwardSpeed => _forwardSpeed;
        public float StrafeSpeed => _strafeSpeed;
        public float SmoothMoveSpeed => _smoothMoveSpeed;

        [Header("View")]
        [SerializeField] private float _sensivitivytiX;
        [SerializeField] private float _sensivitivytiY;
        [SerializeField] private float _viewSmoothSpeed;
        [SerializeField] private float _clampX;
        [SerializeField] private bool _inverseX;
        [SerializeField] private bool _inverseY;
        public float SensivitivytiX => _sensivitivytiX;
        public float SensivitivytiY => _sensivitivytiY;
        public float ViewSmoothSpeed => _viewSmoothSpeed;
        public float ClampX => _clampX;
        public bool InverseX => _inverseX;
        public bool InverseY => _inverseY;

        [Header("Gravity")]
        [SerializeField] private float _gravityAmmount;
        [SerializeField] private float _minGravity;
        public float GravityAmmount => _gravityAmmount;
        public float MinGravity => _minGravity;

        [Header("Jump")]
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpSmoothTime;
        [SerializeField] private float _fallingSmoothSpeed;
        public float JumpHeight => _jumpHeight;
        public float JumpSmoothTime => _jumpSmoothTime;
        public float FallingSmoothSpeed => _fallingSmoothSpeed;

        [Header("Sprint")]
        [SerializeField] private float _sprintForwardSpeed;
        [SerializeField] private float _sprintStrafeSpeed;
        public float SprintForwardSpeed => _sprintForwardSpeed;
        public float SprintStrafeSpeed => _sprintStrafeSpeed;

        [Header("Stance Smoothing")]
        [SerializeField] private float _cameraSmoothingSpeed;
        [SerializeField] private float _heightCharacterSmoothingSpeed;
        [SerializeField] private float _centerCharacterSmoothingSpeed;
        public float CameraSmoothingSpeed => _cameraSmoothingSpeed;
        public float HeightCharacterSmoothingSpeed => _heightCharacterSmoothingSpeed;
        public float CenterCharacterSmoothingSpeed => _centerCharacterSmoothingSpeed;

        [Header("Speed Effector")]
        [SerializeField] private float _speedEffector = 1f;
        [SerializeField] private float _speedEffectorCrouch;
        [SerializeField] private float _speedEffectorProne;
        [SerializeField] private float _SpeedEffectorFalling;
        public float SpeedEffector => _speedEffector;
        public float SpeedEffectorCrouch => _speedEffectorCrouch;
        public float SpeedEffectorProne => _speedEffectorProne;
        public float SpeedEffectorFalling => _SpeedEffectorFalling;

    }

    #endregion


    #region - Weapons -

    [Serializable]
    public class WeaponSettings
    {
        [Header("General")]
        [SerializeField] private float _fireRate;
        [SerializeField] private float _reloadDelay;
        [SerializeField] private float _autoReloadDelay;
        [SerializeField] private int _clipSize;
        [SerializeField] private bool _autoReload;
        public float FireRate => _fireRate;
        public float AutoReloadDelay => _autoReloadDelay;
        public float ReloadDelay => _reloadDelay;
        public int ClipSize => _clipSize;
        public bool AutoReload => _autoReload;

        [Header("Bullet")]
        [SerializeField] private float _bulletForce;
        public float BulletForce => _bulletForce;
    }

    public enum ShootingType
    {
        Single,
        Automatic
    }

    #endregion
}
