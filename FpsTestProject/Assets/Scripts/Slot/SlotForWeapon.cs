using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotForWeapon : MonoBehaviour,IListener
{
    [Header("Inventory")]
    [SerializeField] private Inventory _inventory;

    private ItemGrid _itemGrid;
    private int _indexSlot;

    public int IndexSlot => _indexSlot;


    private void Awake()
    {
        _itemGrid = GetComponent<ItemGrid>();
        _indexSlot = GetComponent<RectTransform>().GetSiblingIndex() - 1;
    }

    public void PlaceInSlot(Gun gun)
    {
        if (_inventory == null)
            return;

        _inventory.AddItem(gun,_indexSlot);
    }


    public void OnEvent(Event_Type eventType, Component sender, Object param = null)
    {
        if (_itemGrid.IsActive == false)
            return;

        Item item = (Item)param;
        Gun gun = (Gun)item;

        PlaceInSlot(gun);
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
