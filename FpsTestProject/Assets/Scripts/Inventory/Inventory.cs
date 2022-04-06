using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] private Transform _parent;

    private Gun[] _weapons;

    private void Start()
    {
        _weapons = new Gun[3];
    }

    public void AddItem(Gun item)
    {
        if(_weapons[(int)item.WeaponStyle] == null)
        {
            _weapons[(int)item.WeaponStyle] = item;
        }
        else
        {
            GameObject gun = Instantiate(_weapons[(int)item.WeaponStyle].Prefab,item.Prefab.transform.position,
                                         item.Prefab.transform.rotation,_parent);
            _weapons[(int)item.WeaponStyle] = item;
        }
    }

    public void RemoveItem(Gun item)
    {
        if(_weapons[(int)item.WeaponStyle] != null)
        {
            _weapons[(int)item.WeaponStyle] = null;
        }
    }

    public Item GetItem(int index)
    {
        if(index >= 0 && index < _weapons.Length)
        {
            if(_weapons[index] != null)
            {
                return _weapons[index];
            }
        }

        return null;
    }
}
