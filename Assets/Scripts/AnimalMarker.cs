using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMarker : ARMarker
{
    public AnimalRuntimeSet RuntimeSet;

    // Start is called before the first frame update

    private void OnEnable()
    {
        base.OnEnable();
        RuntimeSet.Add(this);
    }

    private void OnDisable()
    {
        base.OnDisable();
        RuntimeSet.Remove(this);
    }
}
