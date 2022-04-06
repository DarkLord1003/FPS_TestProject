using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperWeapon : Weapon,IListener
{
    private bool _realisedAim;

    private void Start()
    {
        ObjectPool.CreatePool(BulletPrefab, "Sniper_Bullet", 10, true);

        CurrentAmmo = WeaponSettings.ClipSize;
        CanShoot = true;
        HandsStartPosition = HandsTransform.localPosition;

        EventManager.Instance.AddListener(Event_Type.Weapon_PullTheShatter, this);
        
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

    protected override void Shoot()
    {
        if (InputManager.ShootIsTrigger && !IsReloading && !ArmsAnimator.GetBool("IsSprinting") && CanShoot && !OutOfAmmo)
        {
            CurrentAmmo--;
           
            ArmsAnimator.Play("Sniper_Shoot", 0, 0f);
           
            PoolComponent bullet = ObjectPool.GetObject("Sniper_Bullet", BulletSpawnPoint.position, BulletSpawnPoint.rotation);
            EventManager.Instance.PostNotification(Event_Type.Weapon_Recoil, this);
            SoundManager.Instance.PlayAudio(AudioType.Sniper_Shoot);
            MuzzleFlash.Play();
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * WeaponSettings.BulletForce;
        }
    }

    protected override void AnimationStanceCheck()
    {
        if (ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Sniper_Shoot"))
        {
            CanShoot = false;
        }
        else
        {
            CanShoot = true;
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
        if (InputManager.ReloadIsTrigger)
        {
            Reloading();
        }
    }

    private void Reloading()
    {
        if (OutOfAmmo || CurrentAmmo < WeaponSettings.ClipSize)
        {
            ArmsAnimator.Play("Sniper_ReloadOutOfAmmo", 0, 0f);

            CurrentAmmo = WeaponSettings.ClipSize;
            OutOfAmmo = false;
            StopCoroutine(AutoReload());
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

    protected override void CheckAiming()
    {
        if (InputManager.AimingPressTrigger && !ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_Running"))
        {
            CanAiming = true;
            _realisedAim = false;
        }
        else if(InputManager.AimingRealisedTrigger || ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_Running"))
        {
            CanAiming = false;
            _realisedAim = true;
        }
    }

    #region - Coroutins -

    protected override IEnumerator AutoReload()
    {
        yield return new WaitForSeconds(WeaponSettings.AutoReloadDelay);

        Reloading();
    }

    private IEnumerator DelayPullTheShatter()
    {
        yield return new WaitForSeconds(1.5f);

        if (_realisedAim)
        {
            CanAiming = false;
        }
        else
        {
            CanAiming = true;
        }
    }

    #endregion

    #region - IListener - 

    public void OnEvent(Event_Type eventType, Component sender, Object param = null)
    {
        if (IsAiming)
        {
            CanAiming = false;
            StartCoroutine(DelayPullTheShatter());
        }
    }

    #endregion

    #region - OnDisable -

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(this);
    }

    #endregion
}
