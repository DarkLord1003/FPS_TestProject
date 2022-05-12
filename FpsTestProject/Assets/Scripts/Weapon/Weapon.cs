using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ModelSettings;

public abstract class Weapon:MonoBehaviour
{
    [Header("Camera")]
    protected Camera ScopeCamera;

    [Header("Name Gun")]
    [SerializeField] protected string NameGun;

    [Header("Hands")]
    protected Transform HandsTransform;
    protected Vector3 HandsStartPosition;

    [Header("InputManager")]
    protected InputManager InputManager;

    [Header("Camera Options")]
    [SerializeField] protected float CameraZoomSpeed;
    [SerializeField] protected float DefaultFov;
    [SerializeField] protected float AimFov;

    [Header("Aiming")]
    [SerializeField] protected float AimingSpeed;
    protected bool CanAiming;
    protected bool RealisedAim;
    [SerializeField] protected Vector3 ScopePosition;

    [Header("Shoot Settings")]
    protected bool CanShoot;

    [Header("Crosshair")]
    protected Crosshair WeaponCrosshair;

    [Header("Settings")]
    [SerializeField] protected WeaponSettings WeaponSettings;
    protected int CurrentAmmo;
    protected bool OutOfAmmo;
    protected float LastFired;

    [Header("Spawn Points")]
    [SerializeField] protected Transform BulletSpawnPoint;

    [Header("Prefabs")]
    [SerializeField] protected PoolComponent BulletPrefab;

    [Header("Particle System")]
    [SerializeField] protected ParticleSystem MuzzleFlash;

    [Header("Animator")]
    protected Animator ArmsAnimator;

    [Header("States")]
    protected bool IsReloading;
    protected bool IsAiming;

    public string NameWeapon
    {
        get => NameGun;
    }

    protected abstract void Shoot();
    protected abstract void Aim();
    protected abstract void Reload();
    protected abstract void CheckAmmoInClip();
    protected abstract void AnimationStanceCheck();
    protected abstract void CheckAiming();
    protected abstract void CheckCrosshair();
    protected abstract void Init();
    protected abstract IEnumerator AutoReload();
}
