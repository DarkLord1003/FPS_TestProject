using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour,IListener
{
    [Header("Parent")]
    [SerializeField] private Transform _parent;

    [Header("Equipment Manager")]
    [SerializeField] private EquipmentManager _equipmentManager;

    [Header("Inventory Contoller")]
    [SerializeField] private InventoryController _inventoryController;

    private Gun[] _weapons;
    private Gun _currentEqupedGun;
    private bool _isListener = false;
    public Gun[] Weapons
    {
        get => _weapons;
        set => _weapons = value;
    }

    private void Start()
    {
        _weapons = new Gun[4];

        if (!_isListener)
        {
            EventManager.Instance.AddListener(Event_Type.DropItem_From_Inventory, this);
            _isListener = true;
        }
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

    public void AddItem(Gun gun,int index)
    {
        if (index < 0 || index > _weapons.Length)
            return;

        if (gun == null)
        {
            Debug.Log("Yes");
            int slot = _inventoryController.SelectedGrid.GetComponent<SlotForWeapon>().IndexSlot;
            ReturnWeaponToInventory(slot);
            return;
        }

        if (_weapons[index] != null)
        {
            Equiped(gun,index);
            return;
        }

        Equiped(gun, index);
    }

    private void Equiped(Gun gun, int index)
    {
        gun.IndexOfTheSlotTheItem = index;
        _weapons[index] = gun;
        _equipmentManager.NextEqupedWeapon = _equipmentManager.GetWeapon(_weapons[index]);
        _equipmentManager.UniquipedWeapon();
        _equipmentManager.SetAnimationState(state: gun.ID);
        _currentEqupedGun = gun;
        StartCoroutine(Delay(_currentEqupedGun));
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

    private void ReturnWeaponToInventory(int slot)
    {
        Debug.Log("Current Weapon: " + _equipmentManager.CurrentEqupedWeapon);
        _equipmentManager.UniquipedWeapon();
        _equipmentManager.SetAnimationState(state: 0);
        _currentEqupedGun = _weapons[slot];
        StartCoroutine(Delay(_currentEqupedGun));
        _weapons[slot] = null;
    }

    private IEnumerator DelayThrowItem(Item item)
    {
        yield return new WaitForSeconds(0.5f);

        ThrowItem(item);

        yield break;
    }

    private IEnumerator Delay(Gun gun)
    {
        yield return new WaitForSeconds(1f);
        _equipmentManager.CurrentEqupedWeapon = _equipmentManager.GetWeapon(gun);
    }

    public void OnEvent(Event_Type eventType, Component sender, Object param = null)
    {
        InventoryItem item = (InventoryItem)param;

        if (item)
        {
            ThrowItem(item.Item);
            Destroy(item.gameObject);
        }
    }

    #region - OnEnable/Disable -

    private void OnEnable()
    {
        if (EventManager.Instance)
        {
            EventManager.Instance.AddListener(Event_Type.DropItem_From_Inventory, this);
            _isListener = true;
        }
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(Event_Type.DropItem_From_Inventory, this);
    }
    #endregion
}
