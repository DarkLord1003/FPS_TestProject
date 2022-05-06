using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HighLighter))]
public class InventoryController : MonoBehaviour
{
    [Header("Input Manager")]
    [SerializeField] private InputManager _inputManager;

    [Header("Inventory")]
    [SerializeField] private GameObject _inventory;

    private ItemGrid _selectedGrid;
    private InventoryItem _selectedInventoryItem;
    private HighLighter _highLighter;
    private InventoryItem _inventoryItem;
    private InventoryItem _itemHighLighting;
    private bool _isOpenInventory;

    public ItemGrid SelectedGrid
    {
        get => _selectedGrid;
        set => _selectedGrid = value;
    }

    private void Awake()
    {
        _highLighter = GetComponent<HighLighter>();

        _isOpenInventory = false;
    }

    private void Update()
    {
        DrawDrag();

        if (Input.GetKeyDown(KeyCode.I))
        {
            if(_selectedInventoryItem!=null)
               return;

            if (_isOpenInventory != true)
            {
                _inputManager.ShootAction.actionMap.Disable();
            }
            else
            {
                _inputManager.ShootAction.actionMap.Enable();
            }

            _isOpenInventory = !_isOpenInventory;
            _inventory.SetActive(_isOpenInventory);
            _selectedGrid = null;
        }

        if (_selectedGrid == null)
        {
            _highLighter.SetVisible(false);
            return;
        }
       
        HandleHighLighter();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonClick();
        }
    }

    private void LeftMouseButtonClick()
    {
        Vector2 position = Input.mousePosition;
        Vector2Int tileGridPosition = _selectedGrid.GetTileGridPosition(position);

        if (_selectedInventoryItem == null)
        {
            _selectedInventoryItem = _selectedGrid.GetItem(tileGridPosition.x, tileGridPosition.y);
        }
        else
        {
            bool completed = _selectedGrid.PlaceItem(_selectedInventoryItem, tileGridPosition.x, 
                                                     tileGridPosition.y,ref _inventoryItem);
            if (completed)
            {
                Item item = _selectedInventoryItem.Item;
                EventManager.Instance.PostNotification(Event_Type.Slot_Placement, this, item);

                _selectedInventoryItem = null;

                if (_inventoryItem != null)
                {
                    _selectedInventoryItem = _inventoryItem;
                    _inventoryItem = null;
                }
            }
        }
    }

    private void DrawDrag()
    {
        if(_selectedInventoryItem != null)
        {
            Vector2 position = Input.mousePosition;
            _selectedInventoryItem.RectTransform.position = position;

        }
    }

    private void HandleHighLighter()
    {
        Vector2Int tileGridPosition = _selectedGrid.GetTileGridPosition(Input.mousePosition);

        if(_selectedInventoryItem == null)
        {
            _itemHighLighting = _selectedGrid.GetInfoAboutItem(tileGridPosition.x, tileGridPosition.y);

            if (_itemHighLighting)
            {
                _highLighter.SetVisible(true);
                _highLighter.SetSize(_itemHighLighting);
                _highLighter.SetPosition(_selectedGrid, _itemHighLighting);
            }
            else
            {
                _highLighter.SetVisible(true);
                _highLighter.SetSize(width: 1, height: 1);
                _highLighter.SetPosition(_selectedGrid);
            }
        }
    }

}
