using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCasingProvider : MonoBehaviour
{
    public void SpawnCasing()
    {
        EventManager.Instance.PostNotification(Event_Type.Spawn_Casing, this);
    }
}
