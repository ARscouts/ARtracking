using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueSpawner : MonoBehaviour
{
    public LocationVariable currentLocation;
    public LocationVariable scaleApprox;
    public FloatVariable maxTrackingDistance;
    public ClueMarker clueMarkerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateClueObject()
    {
        RandomInSquare(maxTrackingDistance.Value, out float Lat, out float Lon);
        ClueMarker cm = Instantiate(clueMarkerPrefab);
        cm.SetLonLat(Lon, Lat);

        //Debug.LogWarning("Added new clue locations - Lat: " + cm.Lat + " Lon: " + cm.Lon);
    }

    private void RandomInSquare(float maxTrackingDistance, out float lat, out float lon) //generates random coordinates in square area
    {
        lat = Random.Range(-maxTrackingDistance, maxTrackingDistance) / scaleApprox.Lat + currentLocation.Lat;
        lon = Random.Range(-maxTrackingDistance, maxTrackingDistance) / scaleApprox.Lon + currentLocation.Lon;
    }
}
