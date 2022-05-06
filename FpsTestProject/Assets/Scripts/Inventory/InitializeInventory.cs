using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeInventory : MonoBehaviour
{
    [Header("Item Grid")]
    [SerializeField] private ItemGrid _itemGrid;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (_itemGrid)
        {
            _itemGrid.ItemSlots = new InventoryItem[_itemGrid.GridWidth, _itemGrid.GridHeight];

            Vector2 size = new Vector2();
            size.x = ItemGrid._tileSizeWidth * _itemGrid.GridWidth;
            size.y = ItemGrid._tileSizeHeight * _itemGrid.GridHeight;

            _itemGrid.RectTransform = _itemGrid.GetComponent<RectTransform>();
            _itemGrid.RectTransform.sizeDelta = size;

            Debug.Log(_itemGrid.ItemSlots.Length);
        }
    }
}
