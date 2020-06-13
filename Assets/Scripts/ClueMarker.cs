using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ClueMarker : ARMarker
{
    public ClueRuntimeSet RuntimeSet;
    public AnimalMarker BelongsTo;

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
