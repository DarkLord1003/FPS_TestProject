using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Input Manager")]
    [SerializeField] private InputManager _inputManager;

    [Header("Inventory")]
    [SerializeField] private Inventory _inventory;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask _itemLayer;

    [Header("Ray Range")]
    [SerializeField] private float _rayRange;

    [Header("Camera")]
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }
    private void Update()
    {
        PickUpItem();
    }

    private void PickUpItem()
    {
        if (_inputManager.UseIsTrigger)
        {
            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit, _rayRange, _itemLayer))
            {
                Gun gun = hit.transform.GetComponent<ItemObject>().Item as Gun;

                if (gun)
                {
                    _inventory.AddItem(gun);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
