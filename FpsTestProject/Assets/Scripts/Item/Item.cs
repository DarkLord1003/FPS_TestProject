using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : ScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField] protected int _id;
    [SerializeField] protected Sprite _icon;
    [SerializeField] protected int _widthTiles;
    [SerializeField] protected int _heightTiles;
    protected int _indexOfTheSlotTheItem;

    public string NameItem => _name;
    public int WidthTiles
    {
        get => _widthTiles;
        set => _widthTiles = value;
    }
    public int HeightTiles
    {
        get => _heightTiles;
        set => _heightTiles = value;
    }
    public Sprite Icon
    {
        get => _icon;
        set => _icon = value;
    }

    public int IndexOfTheSlotTheItem
    {
        get => _indexOfTheSlotTheItem;
        set => _indexOfTheSlotTheItem = value;
    }

}
