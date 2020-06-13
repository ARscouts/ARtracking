using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ARMarker : MonoBehaviour
{
    public float Lon;
    public float Lat;
    public float LonInMeters;
    public float LatInMeters;

    public GameObject MeshPrefab;

    //public ARMarkerRuntimeSet ActiveMarkers;
    public ARMarkerRuntimeSet HiddenMarkers;
    public ARMarkerRuntimeSet ActiveMarkers;
    public ARMarkerRuntimeSet Markers;

    public bool isSpawned = false;

    protected void OnEnable()
    {
        Markers.Add(this);
        HiddenMarkers.Add(this);
    }

    protected void OnDisable()
    {
        Markers.Remove(this);
        HiddenMarkers.Remove(this);
        ActiveMarkers.Remove(this);
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
