using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : ScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField] protected int _id;
    [SerializeField] protected Sprite _icon;
}
