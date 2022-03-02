using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Input Manager")]
    [SerializeField] private InputManager _input;

    [Header("Settings")]
    [SerializeField] private float _swayAmmount;
    [SerializeField] private float _swaySmoothing;
    [SerializeField] private float _swayResetSmoothing;
    [SerializeField] private float _clampSwayY;
    [SerializeField] private float _clampSwayX;
    [SerializeField] private bool _swayInverseY;
    [SerializeField] private bool _swayInverseX;

    private Vector3 _targetWeaponRotation;
    private Vector3 _targetWeaponRotationVelocity;

    private Vector3 _newWeaponRotation;
    private Vector3 _newWeaponRotationVelocity;

    void LateUpdate()
    {
        Sway();
    }

    private void Sway()
    {
        _targetWeaponRotation.x += _swayAmmount * (_swayInverseY ? -_input.ViewInput.y :
                                                                _input.ViewInput.y) * Time.deltaTime;

        _targetWeaponRotation.y += _swayAmmount * (_swayInverseX ? _input.ViewInput.x :
                                                                -_input.ViewInput.x) * Time.deltaTime;
        _targetWeaponRotation.z = 2;

        _targetWeaponRotation.x = Mathf.Clamp(_targetWeaponRotation.x, -_clampSwayX, _clampSwayX);
        _targetWeaponRotation.y = Mathf.Clamp(_targetWeaponRotation.y, -_clampSwayY, _clampSwayY);

        _targetWeaponRotation = Vector3.SmoothDamp(_targetWeaponRotation, Vector3.zero, ref _targetWeaponRotationVelocity,
                                                 _swayResetSmoothing);
        _newWeaponRotation = Vector3.SmoothDamp(_newWeaponRotation, _targetWeaponRotation, ref _newWeaponRotationVelocity,
                                              _swaySmoothing);

        transform.localRotation = Quaternion.Euler(_newWeaponRotation);
    }
}
