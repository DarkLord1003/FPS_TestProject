using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private Item _item;

    public Item Item
    {
        get => _item;
    }

}
