using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance = null;
    private Dictionary<Event_Type, List<IListener>> listeners = new Dictionary<Event_Type, List<IListener>>();

    public static EventManager Instance
    {
        get=>_instance;
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    public void AddListener(Event_Type eventType,IListener listener)
    {
        List<IListener> listListen = null;

        if(listeners.TryGetValue(eventType,out listListen))
        {
            listListen.Add(listener);
            return;
        }

        listListen = new List<IListener>();
        listListen.Add(listener);
        listeners.Add(eventType, listListen);
    }

    public void RemoveListener(Event_Type eventType,IListener listener)
    {
        List<IListener> list = new List<IListener>();

        foreach(IListener listener1 in listeners[eventType])
        {
            if(listener1 != listener)
            {
                list.Add(listener1);
            }

        }

        listeners[eventType] = list;

    }

    public void PostNotification(Event_Type eventType,Component sender,Object param = null)
    {
        List<IListener> listListen = null;

        if (!listeners.TryGetValue(eventType, out listListen))
            return;

        for(int i = 0; i < listListen.Count; i++)
        {
            if (!listListen[i].Equals(null))
            {
                listListen[i].OnEvent(eventType, sender, param);
            }
        }
    }

    public void RemoveEvent(Event_Type eventType)
    {
        listeners.Remove(eventType);
    }

    public void RemoveRedundancies()
    {
        Dictionary<Event_Type, List<IListener>> tmpDic = new Dictionary<Event_Type, List<IListener>>();

        foreach(KeyValuePair<Event_Type,List<IListener>> item in listeners)
        {
            for(int i = item.Value.Count-1;i>=0;i--)
            {
                if (item.Value[i].Equals(null))
                    item.Value.RemoveAt(i);
            }

            if(item.Value.Count > 0)
            {
                tmpDic.Add(item.Key, item.Value);
            }
        }

        listeners = tmpDic;
    }

    private void OnLevelWasLoad()
    {
        RemoveRedundancies();
    }

}
