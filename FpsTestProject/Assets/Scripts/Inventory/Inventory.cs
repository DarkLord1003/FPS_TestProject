using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] private Transform _parent;

    [Header("Equipment Manager")]
    [SerializeField] private EquipmentManager _equipmentManager;


    private Gun[] _weapons;

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

    private void ThrowItem(Item item)
    {
        Gun gun = item as Gun;
     
        if (gun != null)
        {
            GameObject obj = Instantiate(gun.Prefab, transform.position + new Vector3(0f,0f,0.5f), transform.rotation);
            Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
            rigidbody.AddForce(Vector3.forward * 2f + Vector3.up * 2f);
            rigidbody.AddTorque(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
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
