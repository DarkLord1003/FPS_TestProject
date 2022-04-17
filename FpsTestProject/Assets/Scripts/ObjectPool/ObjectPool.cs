using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool 
{
    private static List<Part> pools = new List<Part>();

    struct Part
    {
        [SerializeField] private string _name;
        [SerializeField] private List<PoolComponent> _prefabs;
        [SerializeField] private bool _resize;
        [SerializeField] private Transform _parent;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public List<PoolComponent> Prefabs
        {
            get => _prefabs;
            set => _prefabs = value;
        }

        public Transform Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public bool Resize
        {
            get => _resize;
            set => _resize = value;
        }

    }

    public static void CreatePool(PoolComponent sample,string name,int count,bool autoResize)
    {
        if (pools == null || count <= 0 || name.Trim() == string.Empty || sample == null)
            return;

        foreach(Part p in pools)
        {
            if(p.Name.CompareTo(name) == 0)
            {
                return;
            }
        }

        Part part = new Part();
        part.Name = name;
        part.Prefabs = new List<PoolComponent>();
        part.Resize = autoResize;
        part.Parent = new GameObject("Pool - " + name).transform;
        

        for(int i = 0; i < count; i++)
        {
            part.Prefabs.Add(AddObject(sample,name,i,part.Parent));
        }

        pools.Add(part);
    }

    private static void AutoResize(Part part,int index)
    {
        part.Prefabs.Add(AddObject(part.Prefabs[0], part.Name, index, part.Parent));
    }

    private static PoolComponent AddObject(PoolComponent sample,string name,int index,Transform parent)
    {
        PoolComponent obj = GameObject.Instantiate(sample) as PoolComponent;
        obj.gameObject.name = name + "-" + index;
        obj.transform.parent = parent;
        obj.gameObject.SetActive(false);
        return obj;
    }

    public static PoolComponent GetObject(string name,Vector3 position,Quaternion rotation)
    {
        if (pools==null)
            return null;

        foreach(Part part in pools)
        {
            if(string.Compare(part.Name,name) == 0)
            {
                foreach(PoolComponent poolComponent in part.Prefabs)
                {
                    if (!poolComponent.isActiveAndEnabled)
                    {
                        poolComponent.transform.rotation = rotation;
                        poolComponent.transform.position = position;
                        poolComponent.gameObject.SetActive(true);
                        return poolComponent;
                    }
                }

                if (part.Resize)
                {
                    AutoResize(part, part.Prefabs.Count);
                    return part.Prefabs[part.Prefabs.Count - 1];
                }
            }
        }

        return null;
    }

    public static void DestroyPool(string name)
    {
        if (pools == null)
            return;

        int i = 0;

        foreach(Part part in pools)
        {
            if (part.Name.Equals(name))
            {
                GameObject.Destroy(part.Parent.gameObject);
                pools.RemoveAt(i);
                return;
            }

            i++;
        }
    }


}
