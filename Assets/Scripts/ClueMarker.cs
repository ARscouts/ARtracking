using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueMarker : ARMarker
{
    public ClueRuntimeSet RuntimeSet;

    // Start is called before the first frame update

    private void OnEnable()
    {
        RuntimeSet.Add(this);
    }

    private void OnDisable()
    {
        RuntimeSet.Remove(this);
    }
}
