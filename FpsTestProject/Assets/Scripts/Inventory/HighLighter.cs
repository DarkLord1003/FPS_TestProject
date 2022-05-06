using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLighter : MonoBehaviour
{
    [SerializeField] private RectTransform _highLighterRectTransform;

    public RectTransform RectTransform
    {
        get => _highLighterRectTransform;
        set => _highLighterRectTransform = value;
    }

    public void SetSize(InventoryItem item)
    {
        Vector2 size = new Vector2();

        size.x = ItemGrid._tileSizeWidth * item.Item.WidthTiles;
        size.y = ItemGrid._tileSizeHeight * item.Item.HeightTiles;

        _highLighterRectTransform.sizeDelta = size;
    }

    public void SetSize(int width = 1,int height = 1)
    {
        Vector2 size = new Vector2();

        size.x = ItemGrid._tileSizeWidth * width;
        size.y = ItemGrid._tileSizeHeight * height;

        _highLighterRectTransform.sizeDelta = size;
    }

    public void SetPosition(ItemGrid targetGrid,InventoryItem item)
    {
        _highLighterRectTransform.SetParent(targetGrid.GetComponent<RectTransform>());

        Vector2 position = targetGrid.CalculateOnTheGridPosition(item, item.OnGridPositionX, item.OnGridPositionY);
       
        _highLighterRectTransform.localPosition = position;
    }

    public void SetPosition(ItemGrid targetGrid)
    {
        _highLighterRectTransform.SetParent(targetGrid.GetComponent<RectTransform>());

        Vector2Int position = targetGrid.GetTileGridPosition(Input.mousePosition);

        _highLighterRectTransform.localPosition = CalculateOnTheGridPosition(position.x, position.y, 32, 32);
    }

    public Vector2 CalculateOnTheGridPosition(int posX, int posY,int width,int height)
    {
        Vector2 position = new Vector2();
        position.x = (posX * ItemGrid._tileSizeWidth + width / 2);
        position.y = -(posY * ItemGrid._tileSizeHeight + height / 2);
        return position;
    }

    public void SetVisible(bool isVisible)
    {
        _highLighterRectTransform.gameObject.SetActive(isVisible);
    }

}
