using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LocationVariable : ScriptableObject
{
    public float Lon;
    public float Lat;

    public void SetValue(float lon, float lat)
    {
        Lon = lon;
        Lat = lat;
    }

    public void SetValue(LocationVariable value)
    {
        Lon = value.Lon;
        Lat = value.Lat;
    }

    public bool Equals(LocationVariable value)
    {
        if(value.Lat == Lat && value.Lon == Lon)
        {
            return true;
        }
        return false;
    }
}
