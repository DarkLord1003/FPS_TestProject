using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunWeapon : Weapon
{
    [Header("AudioTypes")]
    [SerializeField] private AudioType _audioTypeShoot;
    [SerializeField] private AudioType _audioTypeEmptyClip;

    private void Start()
    {
        ObjectPool.CreatePool(BulletPrefab, "Spas12_Bullet", 10, true);

        CurrentAmmo = WeaponSettings.ClipSize;
        OutOfAmmo = false;
        CanShoot = true;
        HandsStartPosition = HandsTransform.localPosition;
    }
    private void Update()
    {
        CheckAmmoInClip();
        CheckAiming();
        //AnimationStanceCheck();
        Aim();
        Reload();
        Shoot();
    }

    protected override void Aim()
    {
        if (CanAiming && !IsReloading)
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

    protected override void AnimationStanceCheck()
    {
        if(ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_ReloadLeftOfAmmo"))
        {
            //IsReloading = true;
        }
        else
        {
            //IsReloading = false;
        }
    }

    protected override void CheckAiming()
    {
        if(InputManager.AimingPressTrigger && !ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_Running"))
        {
            CanAiming = true;
        }
        else if(InputManager.AimingRealisedTrigger || ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_Running"))
        {
            CanAiming = false;
        }
    }

    protected override void CheckAmmoInClip()
    {
        if(CurrentAmmo == 0)
        {
            OutOfAmmo = true;
        }
    }

    protected override void Reload()
    {
        if (InputManager.ReloadIsTrigger)
        {
            if(CurrentAmmo <WeaponSettings.ClipSize || OutOfAmmo)
            {
                CanAiming = false;
                StartCoroutine(Reloading());
            }
        }
    }

    protected override void Shoot()
    {
        if (InputManager.ShootIsTrigger && !IsReloading && !ArmsAnimator.GetBool("IsSprinting") && CanShoot && !OutOfAmmo)
        {
            CurrentAmmo--;

            ArmsAnimator.Play(NameGun + "_Shoot", 0, 0f);

            PoolComponent bullet = ObjectPool.GetObject("Spas12_Bullet", BulletSpawnPoint.position, BulletSpawnPoint.rotation);
            EventManager.Instance.PostNotification(Event_Type.Weapon_Recoil, this);
            SoundManager.Instance.PlayAudio(_audioTypeShoot);
            MuzzleFlash.Play();
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * WeaponSettings.BulletForce;
        }
        else if (InputManager.ShootIsTrigger && !IsReloading && !ArmsAnimator.GetBool("IsSprinting") && CanShoot && OutOfAmmo
                && !ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_Shoot"))
        {
            AudioSource audioSource = SoundManager.Instance.GetAudioSource(_audioTypeEmptyClip);

            if (audioSource)
            {
                if (!audioSource.isPlaying)
                {
                    SoundManager.Instance.PlayAudio(_audioTypeEmptyClip);
                }
            }
        }
    }

    private IEnumerator Reloading()
    {
        while(CurrentAmmo < WeaponSettings.ClipSize)
        {
            ArmsAnimator.Play(NameGun + "_Reload", 0, 0f);
            CurrentAmmo += 1;
            IsReloading = true;
            yield return new WaitForSeconds(WeaponSettings.ReloadDelay);
        }

        OutOfAmmo = false;
        IsReloading = false;
        yield break;
    }
    protected override IEnumerator AutoReload()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
