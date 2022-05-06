using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotForWeapon : MonoBehaviour,IListener
{
    [Header("Inventory")]
    [SerializeField] private Inventory _inventory;

    private ItemGrid _itemGrid;
    private int _indexSlot;

    private void Awake()
    {
        _itemGrid = GetComponent<ItemGrid>();
        _indexSlot = GetComponent<RectTransform>().GetSiblingIndex() - 1;
    }

    public void PlaceInSlot(Gun gun,ItemGrid selectedSlot)
    {
        if (_inventory == null)
            return;

        gun.IndexOfTheSlotTheItem = _indexSlot;
        _inventory.AddItem(gun,selectedSlot,_indexSlot);
    }


    public void OnEvent(Event_Type eventType, Component sender, Object param = null)
    {
        if (_itemGrid.IsActive == false)
            return;

        InventoryController inventoryController = (InventoryController)sender;

        Item item = (Item)param;
        Gun gun = (Gun)item;

        if (gun && inventoryController)
        {
            PlaceInSlot(gun,inventoryController.SelectedGrid);
        }
    }


    #region - OnEnable/Disable

    private void OnEnable()
    {
        EventManager.Instance.AddListener(Event_Type.Slot_Placement, this);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(Event_Type.Slot_Placement, this);
    }

    #endregion

}
