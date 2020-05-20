using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueSpawner : MonoBehaviour
{
    public LocationVariable currentLocation;
    public LocationVariable scaleApprox;
    public FloatVariable maxTrackingDistance;
    public IntVariable clueCount;
    public ClueMarker clueMarkerPrefab;
    // Start is called before the first frame update

    //public Text DebugText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateClueObjects()
    {
        for (int i = 1; i <= clueCount.Value; i++)
        {
            RandomInSquare(maxTrackingDistance.Value, out float LatInMeters, out float LonInMeters);
            ClueMarker cm = Instantiate(clueMarkerPrefab);
            cm.SetLonLat(LonInMeters / scaleApprox.Lon + currentLocation.Lon, 
                LatInMeters / scaleApprox.Lon + currentLocation.Lat,
                LonInMeters,
                LatInMeters);
            //--------Name and tag of created clues
            cm.name = "ClueObject" + i;

            //--------Debug Text (comment if no need)
            //DebugText.gameObject.SetActive(true);
            //DebugText.text += "\nClueObject" + i + "\nLat: " + cm.Lon + "\nLon: " + cm.Lat;

            //Debug.LogWarning("Added new clue locations - Lat: " + cm.Lat + " Lon: " + cm.Lon);
        }
    }

    private void RandomInSquare(float maxTrackingDistance, out float LatInMeters, out float LonInMeters) //generates random coordinates in square area
    {
        LatInMeters = Random.Range(-maxTrackingDistance, maxTrackingDistance);
        LonInMeters = Random.Range(-maxTrackingDistance, maxTrackingDistance);
    }
}
