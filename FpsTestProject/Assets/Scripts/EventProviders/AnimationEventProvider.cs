using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventProvider : MonoBehaviour
{
    public void PullTheShatter()
    {
        EventManager.Instance.PostNotification(Event_Type.Weapon_PullTheShatter, this);
    }
}
