using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameObjectSet : ScriptableObject
{

    public List<GameObject> Items = new List<GameObject>();

    public GameObject GetRandomItem()
    {
        int ret = UnityEngine.Random.Range(0, Items.Count);

        return Items[ret];
    }
}