using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Input Manager")]
    [SerializeField] private InputManager _inputManager;

    [Header("Inventory Grid")]
    [SerializeField] private ItemGrid _itemGrid;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask _itemLayer;

    [Header("Ray Range")]
    [SerializeField] private float _rayRange;

    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("CanVas Transform")]
    [SerializeField] private Transform _canvasTransform;

    [Header("Prefab Item")]
    [SerializeField] private GameObject _prefabItem;

    private void Update()
    {
        PickUpItem();
    }

    private void PickUpItem()
    {
        if (_inputManager.PickupItemIsTrigger)
        {
            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit, _rayRange, _itemLayer))
            {
                Item item = hit.transform.GetComponent<ItemObject>().Item;

                if (item)
                {
                    Vector2Int? positionForNewItem = _itemGrid.SerachSpaceForItem(item);

                    Debug.Log(positionForNewItem);
                    if (positionForNewItem != null)
                    {
                        Vector2Int position = positionForNewItem.GetValueOrDefault();

                        InventoryItem inventoryItem = Instantiate(_prefabItem).GetComponent<InventoryItem>();
                        inventoryItem.RectTransform.SetParent(_canvasTransform);

                        inventoryItem.SetItem(item);

                        bool compled =_itemGrid.PlaceItem(inventoryItem, position.x, position.y);

                        if (compled)
                        {
                            Destroy(hit.transform.gameObject);
                        }

                    }
                }
            }
        }
    }
}
