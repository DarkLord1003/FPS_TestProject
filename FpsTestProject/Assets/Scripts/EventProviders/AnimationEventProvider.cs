using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventProvider : MonoBehaviour
{
    [SerializeField] private EquipmentManager _equipmentManager;

    public void PullTheShatter()
    {
        EventManager.Instance.PostNotification(Event_Type.Weapon_PullTheShatter, this);
    }

    public void EqupedWeapon()
    {
        _equipmentManager.NextEqupedWeapon.gameObject.SetActive(true);
    }

    public void UnequipedWeapon()
    {
        if(_equipmentManager.CurrentEqupedWeapon != null)
        {
            Debug.Log(123456789);
            _equipmentManager.CurrentEqupedWeapon.gameObject.SetActive(false);
        }
    }
}
