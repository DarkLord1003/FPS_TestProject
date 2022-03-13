using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ModelSettings;

public abstract class Weapon:MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] protected Camera ScopeCamera;

    [Header("Camera Options")]
    [SerializeField] protected float CameraZoomSpeed;
    [SerializeField] protected float DefaultFov;
    [SerializeField] protected float AimFov;

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
    [SerializeField] protected Animator ArmsAnimator;

    [Header("States")]
    protected bool IsReloading;
    protected bool IsAiming;

    protected abstract void Shoot();
    protected abstract void Aim();
    protected abstract void Reload();
    protected abstract void CheckAmmoInClip();
    protected abstract IEnumerator AutoReload();
}
