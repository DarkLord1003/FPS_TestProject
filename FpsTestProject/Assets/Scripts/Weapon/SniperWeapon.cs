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
        if (_input.ShootIsTrigger && !IsReloading && !Animator.GetBool("isSprinting") && _canShoot && !OutOfAmmo)
        {
            if(Time.time > LastFired - 1 / WeaponSettings.FireRate)
            {
                LastFired = Time.time;
                CurrentAmmo--;

                Animator.Play("Fire_Sniper", 0, 0f);

                PoolComponent bullet = ObjectPool.GetObject("Bullet", BulletSpawnPoint.position, BulletSpawnPoint.rotation);

                //ShootAudioSource.clip = ShootSound;
                //ShootAudioSource.Play();
                SoundManager.Instance.PlayAudio(AudioType.Weapon_Shoot);
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * WeaponSettings.BulletForce;

            }
        }
    }

    private void AnimationStanceCheck()
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Fire_Sniper"))
        {
            _canShoot = false;
        }
        else
        {
            _canShoot = true;
        }

        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("ReloadOutOfAmmo_Sniper"))
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
                Animator.Play("ReloadOutOfAmmo_Sniper", 0, 0f);

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
        if (_input.AimingPressTrigger && !Animator.GetCurrentAnimatorStateInfo(0).IsName("Running_Sniper"))
        {
            _canAiming = true;
        }
        else if(_input.AimingRealisedTrigger || Animator.GetCurrentAnimatorStateInfo(0).IsName("Running_Sniper"))
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
