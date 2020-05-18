using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ManagerScriptableObject", order = 1)]
public class SimpleGameManager: ScriptableObject
{
    public float maxTrackingDistance; //How far tracked animal can be placed
    public float minTrackingDistance; //Minimal distance to animal
}
