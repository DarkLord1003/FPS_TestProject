using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolWeapon : Weapon
{
    [Header("Audio Types")]
    [SerializeField] private AudioType _audioTypeShoot;
    [SerializeField] private AudioType _audioTypeEmpty;

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        ObjectPool.CreatePool(BulletPrefab,"Pistol_Bullets", 12, true);

        CurrentAmmo = WeaponSettings.ClipSize;
        OutOfAmmo = false;
        CanShoot = true;
        HandsStartPosition = HandsTransform.localPosition;
    }
    private void Update()
    {
        CheckAmmoInClip();
        Reload();
        AnimationStanceCheck();
        CheckAiming();
        Aim();
        CheckCrosshair();
        Shoot();
    }


    protected override void Shoot()
    {
        if(InputManager.ShootIsTrigger && !ArmsAnimator.GetBool("IsSprinting") && !OutOfAmmo && CanShoot && !IsReloading)
        {
            if((Time.time - LastFired) > 1f / WeaponSettings.FireRate)
            {
                LastFired = Time.time;

                CurrentAmmo--;
                ArmsAnimator.Play(NameGun + "_Shoot", 0, 0f);

                PoolComponent bullet = ObjectPool.GetObject("Pistol_Bullets", BulletSpawnPoint.position, BulletSpawnPoint.rotation);
                EventManager.Instance.PostNotification(Event_Type.Weapon_Recoil, this);
                SoundManager.Instance.PlayOneShot(_audioTypeShoot);
                MuzzleFlash.Play();
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * WeaponSettings.BulletForce;
            }
        }
        else if(InputManager.ShootIsTrigger && !ArmsAnimator.GetBool("IsSprinting") && CanShoot && OutOfAmmo)
        {
            AudioSource audioSource = SoundManager.Instance.GetAudioSource(_audioTypeEmpty);

            if (audioSource)
            {
                if (!audioSource.isPlaying)
                {
                    SoundManager.Instance.PlayAudio(_audioTypeEmpty);
                }
            }
        }
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

    protected override void CheckAiming()
    {
        if(InputManager.AimingPressTrigger && !ArmsAnimator.GetBool("IsSprinting"))
        {
            CanAiming = true;
        }
        else if(InputManager.AimingRealisedTrigger || ArmsAnimator.GetBool("IsSprinting"))
        {
            CanAiming = false;
        }
    }

    protected override void CheckCrosshair()
    {
        if (IsAiming)
        {
            WeaponCrosshair.HideCrosshair(true);
        }
        else
        {
            WeaponCrosshair.HideCrosshair(false);
        }
    }
    protected override void CheckAmmoInClip()
    {
        if(CurrentAmmo == 0)
        {
            OutOfAmmo = true;
        }
        else
        {
            OutOfAmmo = false;
        }
    }

    protected override void AnimationStanceCheck()
    {
        if(ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_ReloadLeftOfAmmo") || 
           ArmsAnimator.GetCurrentAnimatorStateInfo(0).IsName(NameGun + "_ReloadOutOfAmmo"))
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
            if(CurrentAmmo < WeaponSettings.ClipSize && !OutOfAmmo)
            {
                ArmsAnimator.Play(NameGun + "_ReloadLeftOfAmmo", 0, 0f);
                CurrentAmmo = WeaponSettings.ClipSize;
            }
            else if (OutOfAmmo)
            {
                ArmsAnimator.Play(NameGun + "_ReloadOutOfAmmo", 0, 0f);
                OutOfAmmo = false;
                CurrentAmmo = WeaponSettings.ClipSize;
            }
        }
    }

    protected override void Init()
    {
        ScopeCamera = GameObject.FindGameObjectWithTag("ScopeCamera").GetComponent<Camera>();
        HandsTransform = GameObject.Find("WeaponHolder").transform;
        InputManager = FindObjectOfType<InputManager>();
        ArmsAnimator = GetComponentInParent<Animator>();
        WeaponCrosshair = FindObjectOfType<Crosshair>();
    }
    protected override IEnumerator AutoReload()
    {
        throw new System.NotImplementedException();
    }
}
