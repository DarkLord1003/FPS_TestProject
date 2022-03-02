using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSoundProvider : MonoBehaviour
{
    public void PlaySoundReloadOne()
    {
        EventManager.Instance.PostNotification(Event_Type.Weapon_Reload1, this);
    }
    
    public void PlaySoundReloadTwo()
    {
        EventManager.Instance.PostNotification(Event_Type.Weapon_Reload2, this);
    }

    public void PlaySoundReloadThree()
    {
        EventManager.Instance.PostNotification(Event_Type.Weapon_Reload3, this);
    }
    
    public void PlaySoundReloadFour()
    {
        EventManager.Instance.PostNotification(Event_Type.Weapon_Reload4, this);
    }
    
    public void PlaySoundReloadFive()
    {
        EventManager.Instance.PostNotification(Event_Type.Weapon_Reload5, this);
    }


}
