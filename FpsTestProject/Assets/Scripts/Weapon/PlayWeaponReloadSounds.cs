using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWeaponReloadSounds : MonoBehaviour, IListener
{
    [Header("Name Gun")]
    [Tooltip("Ќазвание оружи€ у которого будут проигрыватьс€ звуки перерезар€дки")]
    [SerializeField] private string _nameGun;

    private int _state = 0;

    [Header("Audio Types")]
    private AudioType _reload1;
    private AudioType _reload2;
    private AudioType _reload3;
    private AudioType _reload4;
    private AudioType _reload5;
    private AudioType _reload6;

    private void Start()
    {
        Initialize();

        if (_state == 0)
        {
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload1, this);
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload2, this);
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload3, this);
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload4, this);
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload5, this);
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload6, this);
        }

        Debug.Log("4848");
    }

    #region - OnEvent - 
    public void OnEvent(Event_Type eventType, Component sender, Object param = null)
    {
        switch (eventType)
        {
            case Event_Type.Weapon_Reload1:
                SoundManager.Instance.PlayOneShot(_reload1);
                break;

            case Event_Type.Weapon_Reload2:
                SoundManager.Instance.PlayOneShot(_reload2);
                break;

            case Event_Type.Weapon_Reload3:
                SoundManager.Instance.PlayOneShot(_reload3);
                break;

            case Event_Type.Weapon_Reload4:
                SoundManager.Instance.PlayOneShot(_reload4);
                break;

            case Event_Type.Weapon_Reload5:
                SoundManager.Instance.PlayOneShot(_reload5);
                break;

            case Event_Type.Weapon_Reload6:
                SoundManager.Instance.PlayOneShot(_reload6);
                break;

            default:
                Debug.Log("“акого типа нет!");
                break;

        }
    }

    #endregion

    #region - Initialize - 
    private void Initialize()
    {
        if(_nameGun.ToLower() == "sniper")
        {
            _reload1 = AudioType.Sniper_Reload1;
            _reload2 = AudioType.Sniper_Reload2;
            _reload3 = AudioType.Sniper_Reload3;
            _reload4 = AudioType.Sniper_Reload4;
            _reload5 = AudioType.Sniper_Reload5;
            _reload6 = AudioType.Sniper_Reload6;
        }

        if(_nameGun.ToLower() == "smg45")
        {
            _reload1 = AudioType.Smg45_Reload1;
            _reload2 = AudioType.Smg45_Reload2;
            _reload3 = AudioType.Smg45_Reload3;
        }

        if (_nameGun.ToLower() == "ak")
        {
            _reload1 = AudioType.AK_Reload1;
            _reload2 = AudioType.AK_Reload2;
            _reload3 = AudioType.AK_Reload3;
        }
        
        if (_nameGun.ToLower() == "spas12")
        {
            _reload1 = AudioType.Spas12_Reload1;
            _reload2 = AudioType.Spas12_Reload2;
        }

    }

    #endregion


    #region - Disable - 

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(Event_Type.Weapon_Reload1,this);
        EventManager.Instance.RemoveListener(Event_Type.Weapon_Reload2,this);
        EventManager.Instance.RemoveListener(Event_Type.Weapon_Reload3,this);
        EventManager.Instance.RemoveListener(Event_Type.Weapon_Reload4,this);
        EventManager.Instance.RemoveListener(Event_Type.Weapon_Reload5,this);
        EventManager.Instance.RemoveListener(Event_Type.Weapon_Reload6,this);
        Debug.Log("Remove listenerr" + gameObject.name);
    }

    private void OnEnable()
    {
        if(EventManager.Instance)
        {
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload1, this);
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload2, this);
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload3, this);
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload4, this);
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload5, this);
            EventManager.Instance.AddListener(Event_Type.Weapon_Reload6, this);
            _state = 1;
        }
    }

    #endregion
}
