using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    public List<T> Items = new List<T>();

    public void Add(T element)
    {
        if (!Items.Contains(element))
            Items.Add(element);
    }

    public void Remove(T element)
    {
        if (Items.Contains(element))
            Items.Remove(element);
    }

    public T GetRandomItem()
    {
        int ret = UnityEngine.Random.Range(0, Items.Count);

        return Items[ret];
    }

    void OnEnable()
    {
        Items.Clear();
    }
}
