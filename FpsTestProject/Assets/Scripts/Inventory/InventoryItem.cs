using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    private RectTransform _rectTransform;

    private Item _item;

    private int _onGridPositionX;
    private int _onGridPositionY;

    public Item Item
    {
        get => _item;
        set => _item = value;
    }
    public RectTransform RectTransform
    {
        get => _rectTransform;
        set => _rectTransform = value;
    }

    public int OnGridPositionX
    {
        get => _onGridPositionX;
        set => _onGridPositionX = value;
    }

    public int OnGridPositionY
    {
        get => _onGridPositionY;
        set => _onGridPositionY = value;
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetItem(Item item)
    {
        _item = item;
        GetComponent<Image>().sprite = Item.Icon;

        Vector2 size = new Vector2();

        size.x = ItemGrid._tileSizeWidth * _item.WidthTiles;
        size.y = ItemGrid._tileSizeHeight * _item.HeightTiles;

        _rectTransform.sizeDelta = size;
    }
}
