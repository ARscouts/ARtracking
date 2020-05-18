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

    public bool IsCloseTo(LocationVariable value, float radius) //TODO uzupełnić funkcje
    {
        bool _retVal = false;

        return _retVal;
    }
    public bool IsCloseTo(float lon, float lat, float radius) //TODO uzupełnić funkcje
    {
        bool _retVal = false;

        return _retVal;
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
