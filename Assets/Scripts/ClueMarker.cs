using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueMarker : MonoBehaviour
{
    public ClueRuntimeSet RuntimeSet;
    public float Lon;
    public float Lat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        RuntimeSet.Add(this);
    }

    private void OnDisable()
    {
        RuntimeSet.Remove(this);
    }

    public void SetLonLat(float lon, float lat)
    {
        Lon = lon;
        Lat = lat;
    }
}
