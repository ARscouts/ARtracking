using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ARMarker : MonoBehaviour
{
    public float Lon;
    public float Lat;
    public float LonInMeters;
    public float LatInMeters;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLonLat(float lon, float lat)
    {
        Lon = lon;
        Lat = lat;
    }

    public void SetLonLat(float lon, float lat, float lonInMeters, float latInMeters)
    {
        Lon = lon;
        Lat = lat;
        LonInMeters = lonInMeters;
        LatInMeters = latInMeters;
    }
}
