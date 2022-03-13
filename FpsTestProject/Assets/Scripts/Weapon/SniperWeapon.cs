using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperWeapon : Weapon
{
    [Header("Input Manager")]
    [SerializeField] private InputManager _input;

    [Header("Hands Transform")]
    [SerializeField] private Transform _handsTransform;

    [Header("Setting Aiming")]
    [SerializeField] private Vector3 _scopePosition;
    [SerializeField] private float _aimingSpeed;

    private Vector3 _handsStartPosition;
    private bool _canShoot;
    private bool _canAiming;

    private void Start()
    {
        ObjectPool.CreatePool(BulletPrefab, "Bullet", 10, true);
        CurrentAmmo = WeaponSettings.ClipSize;
        _canShoot = true;
        _handsStartPosition = _handsTransform.localPosition;
        
    }

    private void Update()
    {
        CheckAmmoInClip();
        CheckAiming();
        Shoot();
        AnimationStanceCheck();
        Aim();
        Reload();
    }

    protected override void Aim()
    {
        if (_canAiming)
        {
            _handsTransform.localPosition = Vector3.Lerp(_handsTransform.localPosition, _scopePosition, 
                                                         _aimingSpeed * Time.deltaTime);
            ScopeCamera.fieldOfView = Mathf.Lerp(ScopeCamera.fieldOfView, AimFov, CameraZoomSpeed * Time.deltaTime);
            IsAiming = true;
        }
        else
        {
            _handsTransform.localPosition = Vector3.Lerp(_handsTransform.localPosition, _handsStartPosition,
                                                         _aimingSpeed * Time.deltaTime);
            ScopeCamera.fieldOfView = Mathf.Lerp(ScopeCamera.fieldOfView, DefaultFov, CameraZoomSpeed * Time.deltaTime);
            IsAiming = false;
        }

    }

    protected override void Shoot()
    {
        if (_input.ShootIsTrigger && !IsReloading && !ArmsAnimator.GetBool("IsSprinting") && _canShoot && !OutOfAmmo)
        {
            if(Time.time > LastFired - 1 / WeaponSettings.FireRate)
            {
                LastFired = Time.time;
                CurrentAmmo--;

                ArmsAnimator.Play("Sniper_Shoot", 0, 0f);

                PoolComponent bullet = ObjectPool.GetObject("Bullet", BulletSpawnPoint.position, BulletSpawnPoint.rotation);
                EventManager.Instance.PostNotification(Event_Type.Weapon_Recoil, this);
                SoundManager.Instance.PlayAudio(AudioType.Weapon_Shoot);
                MuzzleFlash.Play();
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * WeaponSettings.BulletForce;

            }
        }
    }

    private void AnimationStanceCheck()
    {
        if (ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Sniper_Shootr"))
        {
            _canShoot = false;
        }
        else
        {
            _canShoot = true;
        }

        if (ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Sniper_ReloadOutOfAmmo"))
        {
            IsReloading = true;
        }
        else
        {
            IsReloading = false;
        }
    }

    protected override void Reload()
    {
        if (_input.ReloadIsTrigger)
        {
            if (OutOfAmmo || CurrentAmmo < WeaponSettings.ClipSize)
            {
                ArmsAnimator.Play("Sniper_ReloadOutOfAmmo", 0, 0f);

                CurrentAmmo = WeaponSettings.ClipSize;
                OutOfAmmo = false;
            }
        }
    }

    protected override void CheckAmmoInClip()
    {
        if(CurrentAmmo == 0)
        {
            OutOfAmmo = true;

            if (WeaponSettings.AutoReload)
            {
                StartCoroutine(AutoReload());
            }
        }
    }

    private void CheckAiming()
    {
        if (_input.AimingPressTrigger && !ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Sniper_Running"))
        {
            _canAiming = true;
        }
        else if(_input.AimingRealisedTrigger || ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Sniper_Running"))
        {
            _canAiming = false;
        }
    }

    protected override IEnumerator AutoReload()
    {
        yield return new WaitForSeconds(WeaponSettings.AutoReloadDelay);

        Reload();
    }
}
