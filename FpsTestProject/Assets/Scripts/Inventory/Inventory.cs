using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] private Transform _parent;

    [Header("Equipment Manager")]
    [SerializeField] private EquipmentManager _equipmentManager;

    [Header("Item Grid")]
    [SerializeField] private ItemGrid _itemGrid;

    private Gun[] _weapons;

    public Gun[] Weapons
    {
        get => _weapons;
        set => _weapons = value;
    }

    private void Start()
    {
        _weapons = new Gun[4];
    }

    public void AddItem(Gun item)
    {
        if(_weapons[(int)item.WeaponStyle] == null)
        {
            _weapons[(int)item.WeaponStyle] = item;
        }
        else
        {
            StartCoroutine(DelayThrowItem(_weapons[(int)item.WeaponStyle]));
            EventManager.Instance.PostNotification(Event_Type.Equiped_Weapon, this, item);
            _weapons[(int)item.WeaponStyle] = item;
        }
    }

    public void AddItem(Gun gun,ItemGrid selectedSlot,int index)
    {
        if (index < 0 || index > _weapons.Length)
            return;

        if(_weapons[index] != null)
        {
            _equipmentManager.CurrentEqupedWeapon = _equipmentManager.GetWeapon(_weapons[index]);
            _weapons[index] = gun;
            _equipmentManager.Eqiped(index);
        }

        _weapons[index] = gun;
        _equipmentManager.UniquipedWeapon();
        _equipmentManager.NextEqupedWeapon = _equipmentManager.GetWeapon(_weapons[index]);

    }

    public void RemoveItem(Gun item)
    {
        if(_weapons[item.IndexOfTheSlotTheItem] != null)
        {
            _weapons[item.IndexOfTheSlotTheItem] = null;
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

    public void ThrowItem(Item item)
    {
        Gun gun = item as Gun;
     
        if (gun != null)
        {
            Debug.Log(1);
            GameObject obj = Instantiate(gun.Prefab, transform.position + (transform.forward), transform.rotation);
            Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
            rigidbody.AddForce(Vector3.forward * 2f + Vector3.up * 2f);
            rigidbody.AddTorque(Random.Range(-100f, 100f), Random.Range(-100, 100f), Random.Range(-100f, 100f));
            obj.transform.parent = _parent;

        }
    }

    private IEnumerator DelayThrowItem(Item item)
    {
        yield return new WaitForSeconds(0.5f);

        ThrowItem(item);

        yield break;
    }
}
