using UnityEngine.EventSystems;
using UnityEngine;

public class ItemGrid : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [Header("Tile Size")]
    public const float _tileSizeWidth = 32f;
    public const float _tileSizeHeight = 32f;

    [Header("Item Grid Settings")]
    [SerializeField] private int _gridWidth = 10;
    [SerializeField] private int _gridHeight = 10;

    [Header("RecTransform")]
    private RectTransform _rectTransform;

    [Header("Inventroy Controller")]
    [SerializeField] private InventoryController _inventoryController;

    [Header("Position Item On The Grid")]
    Vector2 _gridPosition = new Vector2();
    Vector2Int _tileOnTheGridPosition;

    private InventoryItem[,] _itemSlots;
    private bool _isActive;

    public int GridWidth
    {
        get => _gridWidth;
    }

    public int GridHeight
    {
        get => _gridHeight;
    }

    public InventoryItem[,] ItemSlots
    {
        get => _itemSlots;
        set => _itemSlots = value;
    }

    public RectTransform RectTransform
    {
        get => _rectTransform;
        set => _rectTransform = value;
    }

    public bool IsActive
    {
        get => _isActive;
    }

    private void Init(int width,int height)
    {
        _itemSlots = new InventoryItem[width, height];
        Vector2 size = new Vector2();

        size.x = width * _tileSizeWidth;
        size.y = height * _tileSizeHeight;

        _rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        _gridPosition.x = mousePosition.x - _rectTransform.position.x;
        _gridPosition.y = _rectTransform.position.y - mousePosition.y;

        _tileOnTheGridPosition.x = (int)(_gridPosition.x / _tileSizeWidth);
        _tileOnTheGridPosition.y = (int)(_gridPosition.y / _tileSizeHeight);

        return _tileOnTheGridPosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem,int posX,int posY,ref InventoryItem item)
    {
        if (BoundryCheck(posX, posY, inventoryItem.Item.WidthTiles, inventoryItem.Item.HeightTiles) == false)
        {
            return false;
        }

        if (CheckOverlaping(posX, posY, inventoryItem.Item.WidthTiles, inventoryItem.Item.HeightTiles, ref item) == false)
        {
            return false;
        }

        if (item != null)
        {
            CleanGridTiles(item);
        }

        RectTransform rectTransform = inventoryItem.RectTransform;
        rectTransform.SetParent(_rectTransform);

        for (int i = 0; i < inventoryItem.Item.WidthTiles; i++)
        {
            for (int j = 0; j < inventoryItem.Item.HeightTiles; j++)
            {
                _itemSlots[posX + i, posY + j] = inventoryItem;
            }
        }

        inventoryItem.OnGridPositionX = posX;
        inventoryItem.OnGridPositionY = posY;

        Vector2 position = CalculateOnTheGridPosition(inventoryItem, posX, posY);

        rectTransform.localPosition = position;

        return true;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        if (BoundryCheck(posX, posY, inventoryItem.Item.WidthTiles, inventoryItem.Item.HeightTiles) == false)
        {
            return false;
        }

        RectTransform rectTransform = inventoryItem.RectTransform;
        rectTransform.SetParent(_rectTransform);

        for (int i = 0; i < inventoryItem.Item.WidthTiles; i++)
        {
            for (int j = 0; j < inventoryItem.Item.HeightTiles; j++)
            {
                _itemSlots[posX + i, posY + j] = inventoryItem;
            }
        }

        inventoryItem.OnGridPositionX = posX;
        inventoryItem.OnGridPositionY = posY;

        Vector2 position = CalculateOnTheGridPosition(inventoryItem, posX, posY);

        rectTransform.localPosition = position;

        return true;
    }

    public Vector2 CalculateOnTheGridPosition(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = (posX * _tileSizeWidth + _tileSizeWidth  * inventoryItem.Item.WidthTiles / 2);
        position.y = -(posY * _tileSizeHeight + _tileSizeHeight  * inventoryItem.Item.HeightTiles / 2);
        return position;
    }

    public InventoryItem GetItem(int posX,int posY)
    {
        InventoryItem item = _itemSlots[posX, posY];

        if (item == null)
            return null;

        CleanGridTiles(item);

        return item;
    }

    public InventoryItem GetInfoAboutItem(int posX,int posY)
    {
        return _itemSlots[posX, posY];
    }

    private void CleanGridTiles(InventoryItem item)
    {
        for (int i = 0; i < item.Item.WidthTiles; i++)
        {
            for (int j = 0; j < item.Item.HeightTiles; j++)
            {
                _itemSlots[item.OnGridPositionX + i, item.OnGridPositionY + j] = null;
            }
        }
    }

    public void Clear()
    {
        for(int i = 0; i < _gridWidth; i++)
        {
            for(int j = 0; j < _gridHeight; j++)
            {
                _itemSlots[i, j] = null;
            }
        }
    }

    private bool CheckPosition(int posX,int posY)
    {
        if(posX < 0 || posY < 0)
        {
            return false;
        }

        if(posX >= _gridWidth || posY >= _gridHeight)
        {
            return false;
        }

        return true;
    }

    private bool BoundryCheck(int posX,int posY,int width,int height)
    {
        if (CheckPosition(posX, posY) == false)
            return false;

        posX += width - 1;
        posY += height - 1;

        if (CheckPosition(posX, posY) == false)
            return false;

        return true;
    }

    private bool CheckOverlaping(int posX,int posY,int width,int height,ref InventoryItem item)
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(_itemSlots[posX + i,posY + j] != null)
                {
                    if (item == null)
                    {
                        item = _itemSlots[posX + i, posY + j];
                    }
                    else
                    {
                        if(item!= _itemSlots[posX +i,posY + j])
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    public Vector2Int? SerachSpaceForItem(Item item)
    {
        int height = _gridHeight - item.HeightTiles + 1;
        int width = _gridWidth - item.WidthTiles + 1;

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if(CheckAviableSpace(x,y,item.WidthTiles,item.HeightTiles) == true)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;

    }

    public bool CheckAviableSpace(int posX,int posY,int width,int height)
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(_itemSlots[posX,posY] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isActive = true;
        _inventoryController.SelectedGrid = this;
        RectTransform.SetSiblingIndex(0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isActive = false;
        _inventoryController.SelectedGrid = null;
    }
}
