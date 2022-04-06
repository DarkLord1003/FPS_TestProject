using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Gun",menuName = "Items/Gun")]
public class Gun : Item
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _clipSize;
    [SerializeField] private int _clipCount;
    [SerializeField] private float _range;
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private WeaponStyle _weaponStyle;

    public GameObject Prefab
    {
        get => _prefab;
        set => _prefab = value;
    }

    public string Name
    {
        get => _name;
    }

    public int ClipSize
    {
        get => _clipSize;
        set => _clipSize = value;
    }

    public int ClipCount
    {
        get => _clipCount;
        set => _clipCount = value;
    }

    public float Range
    {
        get => _range;
        set => _range = value;
    }

    public WeaponType WeaponType
    {
        get => _weaponType;
        set => _weaponType = value;
    }

    public WeaponStyle WeaponStyle
    {
        get => _weaponStyle;
        set => _weaponStyle = value;
    }
}

#region - Enums -
public enum WeaponType
{
    Hands,
    Pistol,
    AssaultRifle,
    Shotgun,
    Sniper
}

public enum WeaponStyle
{
    Primary,
    Secondary,
    Hands
}

#endregion
