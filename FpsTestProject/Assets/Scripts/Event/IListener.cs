using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Event_Type
{
    Game_End,
    Spawn_Casing,
    Weapon_PullTheShatter,
    Weapon_Recoil,
    Weapon_Reload1,
    Weapon_Reload2,
    Weapon_Reload3,
    Weapon_Reload4,
    Weapon_Reload5,
    Weapon_Reload6,
    Health_Change,
    Equiped_Weapon,
    Crosshair_Resizing,
    Slot_Placement,
    DropItem_From_Inventory
}
public interface IListener
{
    public void OnEvent(Event_Type eventType, Component sender, Object param = null);
}
