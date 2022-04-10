using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ModelSettings;

public class AutomaticWeapon : Weapon
{
    [Header("Shooting Type")]
    private ShootingType _shootingType;

    [Header("Shooting Audio Type")]
    [SerializeField] private AudioType _audioTypeShoot;
    [SerializeField] private AudioType _audioTypeEmptyClip;

    private bool _realisedAim;

    private void Start()
    {
        ObjectPool.CreatePool(BulletPrefab, "Automatic_Bullet", 30, true);

        CurrentAmmo = WeaponSettings.ClipSize;
        _shootingType = ShootingType.Automatic;
        CanShoot = false;
    }


    private void Update()
    {
        CheckHold();
        CheckAmmoInClip();
        ChangeShootType();
        CheckAiming();
        Aim();
        Shoot();
        Reload();
        AnimationStanceCheck();
    }


    protected override void Aim()
    {
        if (CanAiming)
        {
            HandsTransform.localPosition = Vector3.Lerp(HandsTransform.localPosition, ScopePosition,
                                                         AimingSpeed * Time.deltaTime);
            ScopeCamera.fieldOfView = Mathf.Lerp(ScopeCamera.fieldOfView, AimFov, CameraZoomSpeed * Time.deltaTime);
            IsAiming = true;
        }
        else
        {
            HandsTransform.localPosition = Vector3.Lerp(HandsTransform.localPosition, HandsStartPosition,
                                                         AimingSpeed * Time.deltaTime);
            ScopeCamera.fieldOfView = Mathf.Lerp(ScopeCamera.fieldOfView, DefaultFov, CameraZoomSpeed * Time.deltaTime);
            IsAiming = false;
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

    protected override void Reload()
    {
        if (InputManager.ReloadIsTrigger)
        {
            Reloading();
        }
    }

    private void Reloading()
    {
        if (OutOfAmmo)
        {
            if (IsAiming)
            {
                StartCoroutine(DelayReloading());
            }

            ArmsAnimator.Play(NameGun + "_ReloadOutOfAmmo", 0, 0f);
            CurrentAmmo = WeaponSettings.ClipSize;
            OutOfAmmo = false;
            StopCoroutine(AutoReload());
        }
        else if(CurrentAmmo < WeaponSettings.ClipSize && !OutOfAmmo)
        {
            if (IsAiming)
            {
                StartCoroutine(DelayReloading());
            }

            ArmsAnimator.Play(NameGun + "_ReloadLeftOfAmmo", 0, 0f);
            CurrentAmmo = WeaponSettings.ClipSize;
        }

    }

    protected override void Shoot()
    {
        if (CanShoot && !IsReloading && !ArmsAnimator.GetBool("IsSprinting") && !OutOfAmmo)
        {
            if (Time.time - LastFired > 1f / WeaponSettings.FireRate)
            {
                CurrentAmmo--;

                LastFired = Time.time;

                ArmsAnimator.Play(NameGun + "_Shoot", 0, 0f);

                PoolComponent bullet = ObjectPool.GetObject("Automatic_Bullet", BulletSpawnPoint.position, BulletSpawnPoint.rotation);
                EventManager.Instance.PostNotification(Event_Type.Weapon_Recoil, this);
                SoundManager.Instance.PlayOneShot(_audioTypeShoot);
                MuzzleFlash.Play();
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * WeaponSettings.BulletForce;

                if(_shootingType == ShootingType.Single)
                {
                    CanShoot = false;
                }

            }
        }
        else if(CanShoot && !IsReloading && !ArmsAnimator.GetBool("IsSprinting") && OutOfAmmo)
        {
            AudioSource audioSource = SoundManager.Instance.GetAudioSource(_audioTypeEmptyClip);

            ArmsAnimator.Play(NameGun + "_Empty_Clip");

            if (audioSource)
            {
                if (!audioSource.isPlaying)
                {
                    SoundManager.Instance.PlayAudio(_audioTypeEmptyClip);
                }
            }
        }
    }

    private void ChangeShootType()
    {
        if (InputManager.ChangeShootTypeIsTrigger)
        {
            if(_shootingType == ShootingType.Automatic)
            {
                _shootingType = ShootingType.Single;
            }
            else
            {
                _shootingType = ShootingType.Automatic;
            }
        }
    }

    private void CheckHold()
    {
        if (InputManager.ShootIsTrigger)
        {
            CanShoot = true;
        }
        else if (InputManager.ShootRealisedTrigger)
        {
            CanShoot = false;
        }
    }

    protected override void CheckAiming()
    {
        if (InputManager.AimingPressTrigger && !ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_Running"))
        {
            CanAiming = true;
            _realisedAim = false;
        }
        else if (InputManager.AimingRealisedTrigger || ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_Running"))
        {
            CanAiming = false;
            _realisedAim = true;
        }
    }

    protected override void AnimationStanceCheck()
    {
        if (ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_ReloadOutOfAmmo") ||
            ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_ReloadLeftOfAmmo"))
        {
            IsReloading = true;
        }
        else
        {
            IsReloading = false;
        }
    }

    protected override IEnumerator AutoReload()
    {
        yield return new WaitForSeconds(WeaponSettings.AutoReloadDelay);

        if (OutOfAmmo)
        {
            if (IsAiming)
            {
                StartCoroutine(DelayReloading());
            }

            ArmsAnimator.Play(NameGun + "_ReloadOutOfAmmo", 0, 0f);
            CurrentAmmo = WeaponSettings.ClipSize;
            OutOfAmmo = false;
        }
        yield break;
    }

    private IEnumerator DelayReloading()
    {
        CanAiming = false;
        yield return new WaitForSeconds(2f);

        if (!_realisedAim)
        {
            CanAiming = true;
        }

        yield break;
    }
}
